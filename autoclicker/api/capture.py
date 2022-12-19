import io
import logging
from time import sleep
import PIL
import cv2
import numpy as np
from werkzeug.utils import secure_filename
from autoclicker import capture_util
from autoclicker.api.util import RequestStatus
from werkzeug.security import check_password_hash, generate_password_hash
from flask import (
    Blueprint, Response, flash, g, redirect, render_template, request, url_for
)
from werkzeug.exceptions import abort
from autoclicker.image_util import ImageType, convert_pil_image, resize

from autoclicker.ui.auth import login_required
from autoclicker.db import get_db
from autoclicker.capture_util import get_all_window_titles, get_hwnd, grab_window_content, stop_capture
from autoclicker.util import Size

bp = Blueprint('capture', __name__, url_prefix='/capture')


@bp.route("/start/")
@login_required
def start():
    capture_util.stop_capture()

    title = request.args.get('title')
    running = capture_util.start_capture(title=title)

    return redirect(url_for('index'))


@bp.route("/stop")
@login_required
def stop():
    capture_util.stop_capture()

    return RequestStatus.SUCCESS.value


@bp.route("/window_titles")
@login_required
def window_titles():
    hwnds = capture_util.get_all_window_titles()

    return hwnds


def gather_img():
    counter = 0
    encode_param = [int(cv2.IMWRITE_JPEG_QUALITY), 30]

    while True:
        try:
            image = capture_util.get_image()

            if not image:
                return

            resize(image, width=512, height=512)
            _, frame = cv2.imencode(
                '.jpg', convert_pil_image(image, ImageType.OPENCV), encode_param)

            yield (b'--frame\r\nContent-Type: image/jpeg\r\n\r\n' + frame.tobytes() + b'\r\n')
            logging.info(f"Streamed frame{'.' * (counter % 3) }")

            counter += 1
            sleep(0.01)
        except Exception as e:
            logging.error(e)


@bp.route("/stream")
@login_required
def stream():
    return Response(gather_img(), mimetype='multipart/x-mixed-replace; boundary=frame')


@bp.route('/preview')
@login_required
def preview():
    return render_template('preview.html')
