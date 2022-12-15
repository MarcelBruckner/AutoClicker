from capture_util import *
from widgets.app import App, Size
import tkinter as tk

if __name__=="__main__":
    window = App( text_box_size=Size(width=50, height=20))

    while not window.is_closed:
        window.update()