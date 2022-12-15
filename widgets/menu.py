from __future__ import annotations

import tkinter as tk
from tkinter.filedialog import askopenfilename, asksaveasfilename

import widgets

class Menu(tk.Menu):
    
    def __init__(self, master: widgets.App):
        super().__init__(master=master, tearoff=0)
        self.app = master
        self.add_cascade(label='File', menu=widgets.Filemenu(self))
