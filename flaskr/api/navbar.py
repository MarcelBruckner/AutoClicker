from flask import Response, send_from_directory
import os
from flask import (
    Blueprint, flash, g, redirect, render_template, request, session, url_for
)
from werkzeug.security import check_password_hash, generate_password_hash
from flaskr.api.util import RequestStatus
from werkzeug.utils import secure_filename

from flaskr.ui.auth import login_required

bp = Blueprint('navbar', __name__, url_prefix='/navbar')
