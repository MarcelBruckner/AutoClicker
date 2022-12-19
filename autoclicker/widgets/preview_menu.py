from __future__ import annotations

import tkinter as tk
from tkinter.filedialog import askopenfilename, asksaveasfilename

import widgets

class PreviewMenu(tk.Menu):

    def __init__(self, master: widgets.Menu):
        super().__init__(master=master, tearoff=0)

        self.app = master.app

        self.add_checkbutton(label="Render preview", variable=self.app.state.render_preview)