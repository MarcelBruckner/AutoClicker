from __future__ import annotations

import tkinter as tk
from tkinter import ttk
from dataclasses import dataclass

from capture_util import get_all_window_titles, get_hwnd, grab_window_content
from util import Size

import app


class InstructionsTextBox(tk.Text):
    def __init__(self, master: app.App, size: Size):
        super().__init__(master, width=size.width, height=size.height)

    def clear(self):
        self.delete("1.0", tk.END)

    def save(self, name):
        instructions = self.get("1.0", tk.END).strip()
        with open(name, "w") as file:
            file.writelines(instructions)

    def load(self, name):
        with open(name, "r") as file:
            instructions = file.read().strip()
        self.clear()
        self.insert("1.0", instructions)
