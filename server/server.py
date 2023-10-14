import os
from flask import Flask, session, request, abort, send_from_directory
from functools import wraps
from secrets import token_urlsafe
from dotenv import load_dotenv, find_dotenv
import os
from collabotronclient import CollabotronClient

load_dotenv(find_dotenv())

APP_SECRET_KEY = os.getenv('APP_SECRET_KEY')
AUTH_CODE = os.getenv('AUTH_CODE')
AUTH_TOKEN = token_urlsafe(30)
UPLOAD_FOLDER = "osu_files"

ALLOWED_EXTENSIONS = ["osu"]

MAP_FILE_NAME = "map.osu"

app = Flask(__name__)


app.secret_key = APP_SECRET_KEY
app.config['UPLOAD_FOLDER'] = UPLOAD_FOLDER
app.config['CLIENTS'] = []
app.config['CURR_HOST'] = None

def find_client(id):
    if id is None:
        return None
    
    for client in app.config['CLIENTS']:
        if client.get_id() == id:
            return client
    return None

def cycle_host():
    app.config['CURR_HOST'] = (app.config['CURR_HOST'] + 1) % len(app.config['CLIENTS'])

def allowed_file(filename):
    return '.' in filename and filename.rsplit('.', 1)[1].lower() in ALLOWED_EXTENSIONS

def login_required(f):
    @wraps(f)
    def decorated_function(*args, **kwargs):
        try:
            if session['auth_token'] is None or session['auth_token'] != AUTH_TOKEN:
                abort(401)
        except:
            abort(401)
        return f(*args, **kwargs)

    return decorated_function

@app.route("/", methods=['GET'])
@login_required
def index():
    client = find_client(session.get('client_id', None))

    if client is None:
        abort(400)

    return {
        "refresh": (not client.is_updated()),
        "is_host": (app.config['CURR_HOST'] == client.get_id())
        }
    

@app.route("/register", methods=['POST'])
def register():
    if request.form.get("password", "") != AUTH_CODE:
        return {"success": False}
    
    if find_client(session.get("client_id", None)) is not None:
        return {"success": False} 
    
    new_id = len(app.config['CLIENTS']) - 1
    new_client = CollabotronClient(new_id, AUTH_TOKEN)
    app.config['CLIENTS'].append(new_client)
    session['client_id'] = new_id
    session['auth_token'] = AUTH_TOKEN

    if app.config['CURR_HOST'] is None:
        app.config['CURR_HOST'] = new_id

    return {"success": True}

@app.route("/upload", methods=['POST'])
@login_required
def upload():
    client = find_client(session.get("client_id", None))
    if client is None or 'file' not in request.files:
        abort(400)

    if client.get_id() != app.config['CURR_HOST']:
        abort(401)

    file = request.files['file']

    if file.filename == '':
        abort(400)

    if file and allowed_file(file.filename):
        file.save(os.path.join(app.config['UPLOAD_FOLDER'], MAP_FILE_NAME))

        for c in app.config['CLIENTS']:
            if c.get_id() != app.config['CURR_HOST']:
                c.require_update()

        cycle_host()
                
        return {"success": True}
    
    return {"success": False}


@app.route("/refresh", methods=['GET'])
@login_required
def refresh():
    client = find_client(session.get("client_id", None))
    if client is None:
        abort(400)

    client.complete_update()

    return send_from_directory(app.config['UPLOAD_FOLDER'], MAP_FILE_NAME)

    
if __name__ == "__main__":
    app.run(host="0.0.0.0", port=80) # Change these parameters to suit your needs.
