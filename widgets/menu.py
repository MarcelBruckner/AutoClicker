from __future__ import annotations

import tkinter as tk
from tkinter.filedialog import askopenfilename, asksaveasfilename

import app
import widgets


class Menu(tk.Menu):

    def __init__(self, master: app.App):
        super().__init__(master=master, tearoff=0)
        self.app = master
        self.add_cascade(label='File', menu=widgets.FileMenu(self))
        self.add_cascade(label='Preview', menu=widgets.PreviewMenu(self))
        self.add_cascade(label='Window', menu=widgets.CaptureWindowMenu(self))
        self.add_cascade(label='Theme', menu=widgets.ThemeMenu(self))
