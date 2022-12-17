from flask import (
    Blueprint, flash, g, redirect, render_template, request, url_for
)
from werkzeug.exceptions import abort

from flaskr.api.auth import login_required
from flaskr.db import get_db

bp = Blueprint('autoclicker', __name__)


@bp.route('/')
def index():
    return render_template('base.html')
