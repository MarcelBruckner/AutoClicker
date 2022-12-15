import sys
import tkinter as tk
from PIL import ImageTk as itk
from typing import Tuple
from collections import namedtuple

from util import Size


class App(tk.Tk):
    """The app window showing the thumbnail, the input field and the buttons.

    Args:
        tk (tkinter.Tk): The tkinter base window class.
    """
    def __init__(self, text_box_size: Size) -> None:
        """constructor

        Args:
            canvas_size (Size): The absolute sive of the canvas in pixels.
            text_box_size (Size): The size of the textbox in characters per line.
        """
        super().__init__()
        
        self.protocol("WM_DELETE_WINDOW", self.on_close_button_clicked)
        self.is_closed = False

        self.text_box = tk.Text(width=text_box_size.width,
                                height=text_box_size.height)
        self.text_box.pack(side=tk.TOP, padx=10)

        button = tk.Button(
            text="Click me!",
            command=self.on_button_callback
        )
        button.pack()

    def on_button_callback(self):
        """Callback for the button.
        """
        print(self.text_box.get("1.0", tk.END))

    def on_close_button_clicked(self):
        """Callback for the CLOSE button.
        """
        self.destroy()
        self.is_closed = True
