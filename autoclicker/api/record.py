from flask import (
    Blueprint, flash, g, redirect, render_template, request, session, url_for
)
from werkzeug.security import check_password_hash, generate_password_hash

import autoclicker.hardware.backup_mouse as backup_mouse
import autoclicker.hardware.my_keyboard as my_keyboard
from autoclicker.api.util import RequestStatus
from autoclicker.ui.auth import login_required


bp = Blueprint('record', __name__, url_prefix='/record')


@bp.route("/start")
@login_required
def start():
    backup_mouse.stop_listen()
    my_keyboard.start()

    return RequestStatus.SUCCESS.value


@bp.route("/stop")
@login_required
def stop():
    backup_mouse.stop_listen()
    my_keyboard.stop()

    return RequestStatus.SUCCESS.value
