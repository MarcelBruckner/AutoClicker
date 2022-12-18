from werkzeug.utils import secure_filename
from flaskr.api.util import RequestStatus
from werkzeug.security import check_password_hash, generate_password_hash
from flask import (
    Blueprint, Response, flash, g, redirect, render_template, request, url_for
)
from werkzeug.exceptions import abort

from flaskr.ui.auth import login_required
from flaskr.db import get_db

bp = Blueprint('autoclicker', __name__)


EXTENSION = ".autoclicker"
ALLOWED_EXTENSIONS = {'txt', 'autoclicker'}

instructions = ''


def allowed_file(filename):
    return '.' in filename and \
           filename.rsplit('.', 1)[1].lower() in ALLOWED_EXTENSIONS


@bp.route('/')
@login_required
def index():
    return render_template('autoclicker.html', instructions=instructions)


@bp.route('/instructions/set', methods=['POST'])
@login_required
def set_instructions():
    global instructions

    instructions = request.form['instructions']

    return "success"


@bp.route("/instructions/save")
@login_required
def save():
    global instructions

    return Response(
        instructions.strip(),
        mimetype="text/plain",
        headers={"Content-disposition":
                 "attachment; filename=.autoclicker"})


@bp.route("/instructions/open", methods=['POST'])
@login_required
def open():
    global instructions

    if request.method == 'POST':
        # check if the post request has the file part
        if 'open' not in request.files:
            flash('No file part')
            return redirect(request.url)
        file = request.files['open']
        # If the user does not select a file, the browser submits an
        # empty file without a filename.
        if file.filename == '':
            flash('No selected file')
            return redirect(request.url)
        if file and allowed_file(file.filename):
            instructions = file.stream.read().decode('UTF-8')
            return redirect(url_for('index'))

    return redirect(request.url)


@bp.route("/instructions/new")
@login_required
def new():
    global instructions

    instructions = ''
    return redirect(url_for('index'))
