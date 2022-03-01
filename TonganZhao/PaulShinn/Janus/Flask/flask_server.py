from flask import Flask

app = Flask(__name__)

#export FLASK_APP=hello
#flask run
#Running on http://127.0.0.1:5000/

@app.route("/")
def hello_world():
    return "<p>Hello, World!</p>"