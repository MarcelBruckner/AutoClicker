from __future__ import annotations

import tkinter as tk
from tkinter.filedialog import askopenfilename, asksaveasfilename

import widgets


class FileMenu(tk.Menu):

    EXTENSION = ".autoclicker"
    FILETYPES = [("AutoClicker Files", EXTENSION)]

    def __init__(self, master: widgets.Menu):
        super().__init__(master=master, tearoff=0)

        self.app = master.app

        self.add_command(
            label="New", command=self.app.on_new)
        self.add_command(label="Save", command=self.on_save)
        self.add_command(label="Open...", command=self.on_load)

        self.add_separator()

        self.add_command(label="Exit", command=self.app.destroy)

    def on_load(self):
        filename = askopenfilename(filetypes=FileMenu.FILETYPES)
        if not filename:
            return
        self.app.on_load(filename)

    def on_save(self):
        filename = asksaveasfilename(filetypes=FileMenu.FILETYPES)
        if not filename:
            return
        if not filename.endswith(FileMenu.EXTENSION):
            filename += FileMenu.EXTENSION
        self.app.on_save(filename)
