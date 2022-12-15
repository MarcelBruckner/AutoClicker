from __future__ import annotations

import tkinter as tk
from tkinter.filedialog import askopenfilename, asksaveasfilename

import widgets

class Filemenu(tk.Menu):

    EXTENSION = ".autoclicker"
    FILETYPES = [("AutoClicker Files", EXTENSION)]

    def __init__(self, master: widgets.Menu):
        super().__init__(master=master, tearoff=0)

        self.app = master.app

        self.add_command(label="New", command=self.app.clear_instructions)
        self.add_command(label="Save", command=self.on_save)
        self.add_command(label="Open...", command=self.on_load)

        self.add_separator()

        self.add_command(label="Exit", command=self.app.close_app)

    def on_load(self):
        filename = askopenfilename(filetypes=Filemenu.FILETYPES)
        if not filename:
            return
        self.app.load_instructions(filename)

    def on_save(self):
        filename = asksaveasfilename(filetypes=Filemenu.FILETYPES)
        if not filename:
            return
        if not filename.endswith(Filemenu.EXTENSION):
            filename += Filemenu.EXTENSION
        self.app.save_instructions(filename)