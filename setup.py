from setuptools import find_packages, setup

setup(
    name='autoclicker',
    version='1.0.0',
    packages=find_packages(),
    include_package_data=True,
    install_requires=[
        'flask',
        'pillow',
        'autopep8',
        'numpy',
        'pywin32',
        'opencv-python',
        'screeninfo',
        'pynput',
        'pyautogui',
        'waitress',  # waitress-serve --call 'autoclicker:create_app'
        'pytest',
        'coverage',
        'greenlet',
        'lark',
        'pydot',
        'graphviz'
    ],
)
