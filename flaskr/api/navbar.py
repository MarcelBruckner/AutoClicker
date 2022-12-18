from flask import (
    Blueprint, flash, g, make_response, redirect, render_template, request, session, url_for
)
from werkzeug.security import check_password_hash, generate_password_hash
from flaskr.capture_util import get_hwnd, start_capture

import flaskr.hardware.backup_mouse as backup_mouse
import flaskr.hardware.my_keyboard as my_keyboard
from flaskr.api.util import RequestStatus
from flaskr.ui.auth import login_required


bp = Blueprint('navbar', __name__, url_prefix='/navbar')
