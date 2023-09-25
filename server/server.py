from flask import Flask, session, request, abort, send_file
from functools import wraps
from secrets import token_urlsafe

AUTH_TOKEN = token_urlsafe(30)

def login_required(f):
    @wraps(f)
    def decorated_function(*args, **kwargs):
        try:
            if (session['auth_token'] is None) or (session['auth_token'] != AUTH_TOKEN):
                abort(401)
        except:
            abort(401)
        return f(*args, **kwargs)

    return decorated_function


class CollabotronServer:
    def __init__(self, app_name, secret_key) -> None:
        self.app = Flask(app_name)
        self.app.secret_key = secret_key
        self.clients = []
        self.host = None