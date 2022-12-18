from flask import (
    Blueprint, flash, g, redirect, render_template, request, session, url_for
)
from werkzeug.security import check_password_hash, generate_password_hash

from flaskr import capture_util
from flaskr.api.util import RequestStatus
from flask import request
from flaskr.ui.auth import login_required

bp = Blueprint('capture', __name__, url_prefix='/capture')


@bp.route("/start/")
@login_required
def start_capture():
    title = request.args.get('title')
    running = capture_util.start_capture(title=title)

    return RequestStatus.SUCCESS.value if running else RequestStatus.FAILURE.value


@bp.route("/stop")
@login_required
def stop_capture():
    capture_util.stop_capture()

    return RequestStatus.SUCCESS.value
