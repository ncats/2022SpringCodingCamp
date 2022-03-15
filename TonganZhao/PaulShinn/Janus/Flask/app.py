from flask import Flask, render_template, request
from werkzeug.utils import secure_filename

app = Flask(__name__)

app.config["UPLOAD_FOLDER"] = "static/"

@app.route('/')
def home():
    return render_template("home.html")

@app.route('/upload')
def upload_file():
    return render_template('upload.html')

@app.route('/about')
def about():
    return render_template("about.html")

@app.route('/upload_file', methods = ['GET', 'POST'])
def display_file():
    if request.method == 'POST':
        f = request.files['file']
        filename = secure_filename(f.filename)

        f.save(app.config['UPLOAD_FOLDER'] + filename)

        file = open(app.config['UPLOAD_FOLDER'] + filename,"r")
        content = file.read()
        volume=request.form.get("Volume")
        dil_points=request.form.get("dil_points")
        instrument=request.form.get("instrument")
        return str(volume) + str(dil_points) + str(instrument)
        
    return render_template('upload.html', content=content) 

if __name__ == '__main__':
    app.run(host="0.0.0.0", port=5000, debug = True)