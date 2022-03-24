from flask import Flask, render_template, request
from werkzeug.utils import secure_filename

import re
import janus

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
        filename = app.config['UPLOAD_FOLDER'] + filename

        #extracts the root file name and appends "worklist" or "platemap" onto it
        worklist=re.findall("(\S+).csv", str(filename))[0] + "-worklist.xlsx"

        janus.readCSVFile2(filename, instrument, int(dil_points), volume, worklist)

        #return volume + dil_points + instrument + filename
        return worklist + " created!"

    return render_template('upload.html', content=content) 

if __name__ == '__main__':
    app.run(host="0.0.0.0", port=5000, debug = True)