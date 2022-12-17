python setup.py bdist_wheel
pip install dist/flaskr-1.0.0-py3-none-any.whl
.\venv\Scripts\activate
flask --app flaskr init-db
flask --app flaskr --debug run