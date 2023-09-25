from dotenv import load_dotenv, find_dotenv
import os


load_dotenv(find_dotenv())

APP_SECRET_KEY = os.getenv('APP_SECRET_KEY')
AUTH_CODE = os.getenv('AUTH_CODE')