from __future__ import annotations

import tkinter as tk
from tkinter.filedialog import askopenfilename, asksaveasfilename
from capture_util import get_all_window_titles

import widgets

class CaptureWindowMenu(tk.Menu):

    def __init__(self, master: widgets.Menu):
        super().__init__(master=master, tearoff=0)

        self.app = master.app
        self.titles = []
        self.selected = ""
        self.selected_index = tk.StringVar(self)
        self.set_options()

    def set_options(self):
        self.titles = get_all_window_titles()
        self.delete(0, tk.END)

        for title in self.titles:
            self.add_radiobutton(label=title, variable=self.app.state.capture_window_title)

        self.add_separator()
        self.add_command(label="Refresh", command=self.set_options)
