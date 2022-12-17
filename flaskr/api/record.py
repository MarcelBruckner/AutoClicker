from flask import (
    Blueprint, flash, g, redirect, render_template, request, session, url_for
)
from werkzeug.security import check_password_hash, generate_password_hash

import flaskr.hardware.mouse as mouse
import flaskr.hardware.keyboard as keyboard
from flaskr.api.util import RequestStatus
from flaskr.api.auth import login_required


bp = Blueprint('record', __name__, url_prefix='/record')


@bp.route("/start")
@login_required
def start():
    mouse.listen()
    keyboard.listen()

    return RequestStatus.SUCCESS.value


@bp.route("/stop")
@login_required
def stop():
    mouse.stop_listen()
    keyboard.stop_listen()

    return RequestStatus.SUCCESS.value
