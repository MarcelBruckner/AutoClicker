from __future__ import annotations

import sys
import tkinter as tk
from collections import namedtuple
from dataclasses import dataclass
from enum import Enum
from tkinter.filedialog import askopenfilename
from typing import List, Tuple

import cv2
from capture_util import (get_all_window_handles, get_all_window_titles,
                          get_hwnd, grab_window_content)
from image_util import ImageType, convert_pil_image
from PIL import ImageTk as itk
from util import Size

import widgets

class Menu(tk.Menu):

    def __init__(self, master: widgets.App):
        super().__init__(master=master)

        self.filemenu = tk.Menu(self, tearoff=0)
        self.filemenu.add_command(label="New", command=self.new_file)
        self.filemenu.add_command(label="Open...", command=self.open_file)
        self.filemenu.add_separator()
        self.filemenu.add_command(label="Exit", command=master.close_app)
        self.add_cascade(label="File", menu=self.filemenu)

    def new_file(self):
        print("New File!")

    def open_file(self):
        name = askopenfilename()
        print(name)
