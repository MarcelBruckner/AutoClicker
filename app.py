from __future__ import annotations

import tkinter as tk
from tkinter import ttk
from dataclasses import dataclass

from capture_util import get_hwnd, grab_window_content
from util import Size

import widgets


class App(tk.Tk):

    @dataclass
    class State:
        render_preview: tk.BooleanVar
        capture_window_title: tk.StringVar
        style: ttk.Style
        hwnd: str = ''
        is_closed: bool = False

    @dataclass
    class Components:
        menu: widgets.Menu
        instructions_text_box: widgets.InstructionsTextBox
        preview_window: widgets.PreviewWindow = None

    """The app window showing the thumbnail, the input field and the buttons.

    Args:
        tk (tkinter.Tk): The tkinter base window class.
    """

    def __init__(self, text_box_size: Size, capture_window_title: str = "RuneScape") -> None:
        """constructor

        Args:
            text_box_size (Size): The size of the textbox in characters per line.
        """
        super().__init__(className='AutoClicker')
        self.protocol("WM_DELETE_WINDOW", self.destroy)

        self.state = App.State(
            style=ttk.Style(self),
            render_preview=tk.BooleanVar(self, False),
            capture_window_title=tk.StringVar(self, capture_window_title),
        )

        self.setup_filemenu_facade()
        self.components = App.Components(
            menu=widgets.Menu(self),
            instructions_text_box=widgets.InstructionsTextBox(
                self, text_box_size),
        )

        self.config(menu=self.components.menu)
        self.state.render_preview.trace(
            mode='w', callback=self.on_render_preview_changed)
        self.state.capture_window_title.trace(
            mode='w', callback=self.on_capture_window_title_changed)

        self.components.instructions_text_box.pack(
            side=tk.TOP, padx=10, pady=10, fill='both', expand=True)

        self.setup_afters()

    @property
    def is_closed(self):
        return self.state.is_closed

    def setup_afters(self):
        self.after(1, self.process_image)

    def setup_filemenu_facade(self):
        self.on_new = lambda: self.components.instructions_text_box.clear()
        self.on_save = lambda name: self.components.instructions_text_box.save(
            name)
        self.on_load = lambda name: self.components.instructions_text_box.load(
            name)
        self.on_close = self.destroy

    def on_render_preview_changed(self, *args):
        if self.state.render_preview.get():
            self.components.preview_window = widgets.PreviewWindow(self)
        else:
            self.components.preview_window.destroy()

    def on_capture_window_title_changed(self, *args):
        try:
            self.state.hwnd = get_hwnd(self.state.capture_window_title.get())
            self.components.capture_window_option_menu.config(
                background='White')
        except:
            pass

    def process_image(self) -> None:
        self.image = grab_window_content(hwnd=self.state.hwnd)
        if self.components.preview_window and self.image is not None and self.state.render_preview:
            self.components.preview_window.update(self.image)

        self.after(1, self.process_image)
