from __future__ import annotations

import tkinter as tk
from tkinter.filedialog import askopenfilename, asksaveasfilename

import widgets


class ThemeMenu(tk.Menu):

    def __init__(self, master: widgets.Menu):
        super().__init__(master=master, tearoff=0)

        self.app = master.app
        self.selected_theme = tk.StringVar(self, self.app.state.style.theme_use())

        for theme_name in self.app.state.style.theme_names():
            self.add_radiobutton(
                label=theme_name,
                variable=self.selected_theme,
                command=self.change_theme )

    def change_theme(self):
        selected_theme = self.selected_theme.get()
        self.app.state.style.theme_use(selected_theme)
