from __future__ import annotations

import tkinter as tk
from dataclasses import dataclass

from capture_util import get_all_window_titles, get_hwnd, grab_window_content
from util import Size

import widgets


class App(tk.Tk):

    @dataclass
    class State:
        render_preview: tk.BooleanVar
        capture_window_title: tk.StringVar
        hwnd: str = ''
        is_closed: bool = False

    @dataclass
    class Components:
        menu: widgets.Menu
        capture_window_option_menu: tk.Entry
        instructions_text_box: tk.Text
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
        self.protocol("WM_DELETE_WINDOW", self.close_app)

        self.state = App.State(
            render_preview=tk.BooleanVar(self, False),
            capture_window_title=tk.StringVar(self, capture_window_title),
        )

        self.components = App.Components(
            menu=widgets.Menu(self),
            capture_window_option_menu=tk.OptionMenu(
                self, self.state.capture_window_title, get_all_window_titles()),
            instructions_text_box=tk.Text(
                self, width=text_box_size.width, height=text_box_size.height),
        )

        self.config(menu=self.components.menu)
        self.state.render_preview.trace(
            mode='w', callback=self.on_render_preview_changed)
        self.state.capture_window_title.trace(
            mode='w', callback=self.on_capture_window_title_changed)
        self.state.capture_window_title.trace(
            mode='r', callback=self.on_capture_window_title_read)

        self.components.capture_window_option_menu.pack()
        self.components.instructions_text_box.pack(
            side=tk.TOP, padx=10, pady=10, fill='both', expand=True)

    @property
    def is_closed(self):
        return self.state.is_closed

    def clear_instructions(self):
        self.components.instructions_text_box.delete("1.0", tk.END)

    def save_instructions(self, name):
        instructions = self.components.instructions_text_box.get(
            "1.0", tk.END).strip()
        with open(name, "w") as file:
            file.writelines(instructions)

    def load_instructions(self, name):
        with open(name, "r") as file:
            instructions = file.read().strip()
        self.clear_instructions()
        self.components.instructions_text_box.insert("1.0", instructions)

    def focus_instructions(self):
        self.components.instructions_text_box.focus()

    def close_app(self):
        """Callback for the CLOSE button.
        """
        self.destroy()
        self.state.is_closed = True

    def on_render_preview_changed(self, *args):
        if self.state.render_preview.get():
            self.components.preview_window = widgets.PreviewWindow(self)
        else:
            self.components.preview_window.destroy()

    def on_capture_window_title_read(self, *args):
        self.components.capture_window_option_menu['menu'].delete(0, tk.END)
        for title in get_all_window_titles():
            self.components.capture_window_option_menu['menu'].add_command(
                label=title, command=tk._setit(self.state.capture_window_title, title))

    def on_capture_window_title_changed(self, *args):
        try:
            self.state.hwnd = get_hwnd(self.state.capture_window_title.get())
            self.components.capture_window_option_menu.config(
                background='White')
        except:
            self.components.capture_window_option_menu.config(background='Red')

    def update(self) -> None:
        self.image = grab_window_content(hwnd=self.state.hwnd)
        if self.components.preview_window and self.image is not None and self.state.render_preview:
            self.components.preview_window.update(self.image)

        super().update()
