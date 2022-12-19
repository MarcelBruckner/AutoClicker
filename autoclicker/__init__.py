import os

from flask import Flask
from hardware import my_mouse, my_keyboard

from autoclicker.capture_util import get_all_window_titles, get_hwnd


def create_app(test_config=None) -> Flask:
    # create and configure the app
    app = Flask(__name__, instance_relative_config=True)
    app.config.from_mapping(
        SECRET_KEY='dev',
        DATABASE=os.path.join(app.instance_path, 'autoclicker.sqlite'),
    )

    if test_config is None:
        # load the instance config, if it exists, when not testing
        app.config.from_pyfile('config.py', silent=True)
    else:
        # load the test config if passed in
        app.config.from_mapping(test_config)

    # ensure the instance folder exists
    try:
        os.makedirs(app.instance_path)
    except OSError:
        pass

    from . import db
    db.init_app(app)

    from autoclicker.api import capture, record, navbar
    from autoclicker.ui import auth, autoclicker

    app.register_blueprint(auth)
    app.register_blueprint(capture)
    app.register_blueprint(record)
    app.register_blueprint(navbar)
    app.register_blueprint(autoclicker)

    app.add_url_rule('/', endpoint='index')

    return app


if __name__ == "__main__":
    create_app().run(threaded=True, debug=True)
