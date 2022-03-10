from flask import Flask, render_template, request
from werkzeug.utils import secure_filename

app = Flask(__name__)

app.config["UPLOAD_FOLDER"] = "static/"

@app.route('/')
def upload_file():
    return render_template('index.html')


@app.route('/display', methods = ['GET', 'POST'])
def display_file():
    if request.method == 'POST':
        f = request.files['file']
        filename = secure_filename(f.filename)

        f.save(app.config['UPLOAD_FOLDER'] + filename)

        file = open(app.config['UPLOAD_FOLDER'] + filename,"r")
        content = file.read()   
        
    return render_template('content.html', content=content) 

@app.route('/display')
def index():
    return render_template('index.html',
        data=[{'gender': 'Gender'}, {'gender': 'female'}, {'gender': 'male'}],
        data1=[{'noc': 'Number of Children'}, {'noc': 0}, {'noc': 1}, {'noc': 2}, {'noc': 3}, {'noc': 4}, {'noc': 5}],
        data2=[{'smoke': 'Smoking Status'}, {'smoke': 'yes'}, {'smoke': 'no'}],
        data3=[{'region': "Region"}, {'region': "northeast"}, {'region': "northwest"},
               {'region': 'southeast'}, {'region': "southwest"}])

if __name__ == '__main__':
    app.run(host="0.0.0.0", port=5000, debug = True)