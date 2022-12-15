from __future__ import annotations

import sys
import tkinter as tk
from collections import namedtuple
from dataclasses import dataclass
from enum import Enum
from tkinter.filedialog import askopenfilename
from typing import List, Tuple

import cv2
from capture_util import (get_all_window_handles, get_all_window_titles,
                          get_hwnd, grab_window_content)
from image_util import ImageType, convert_pil_image
from PIL import ImageTk as itk
from util import Size

import widgets


class App(tk.Tk):

    @dataclass
    class State:
        render_preview: tk.BooleanVar
        capture_window_title: tk.StringVar
        hwnd: str
        is_closed: bool = False

    @dataclass
    class Components:
        menu: widgets.Menu
        render_preview_check_button: tk.Checkbutton
        capture_window_option_menu: tk.Entry
        instructions_text_box: tk.Text

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
        self.title = 'AutoClicker'
        self.protocol("WM_DELETE_WINDOW", self.close_app)

        self.state = App.State(
            render_preview=tk.BooleanVar(self, False),
            capture_window_title=tk.StringVar(self, capture_window_title),
            hwnd=get_hwnd(capture_window_title),
        )

        self.components = App.Components(
            menu=widgets.Menu(self),
            render_preview_check_button=tk.Checkbutton(
                self, text='Render preview', variable=self.state.render_preview),
            capture_window_option_menu=tk.OptionMenu(
                self, self.state.capture_window_title, get_all_window_titles()),
            instructions_text_box=tk.Text(
                self, width=text_box_size.width, height=text_box_size.height),
        )

        self.config(menu=self.components.menu)
        self.state.render_preview.trace(
            mode='rw', callback=self.on_render_preview_changed)
        self.state.capture_window_title.trace(
            mode='w', callback=self.on_capture_window_title_changed)
        self.state.capture_window_title.trace(
            mode='r', callback=self.on_capture_window_title_read)

        self.components.render_preview_check_button.pack()
        self.components.capture_window_option_menu.pack()
        self.components.instructions_text_box.pack(side=tk.TOP, padx=10)

    @property
    def is_closed(self):
        return self.state.is_closed

    def close_app(self):
        """Callback for the CLOSE button.
        """
        self.destroy()
        self.state.is_closed = True

    def on_render_preview_changed(self, *args):
        if self.state.render_preview.get():
            cv2.namedWindow("Preview", cv2.WINDOW_NORMAL)
        else:
            cv2.destroyAllWindows()

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
        if self.image is not None and self.state.render_preview.get():
            cv2_image = convert_pil_image(
                img=self.image, image_type=ImageType.OPENCV)
            cv2.imshow("Preview", cv2_image)
            cv2.waitKey(1)

        super().update()
