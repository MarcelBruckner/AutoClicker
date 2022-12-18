from __future__ import annotations

import tkinter as tk

import app
import hardware


class Controls(tk.Frame):
    def __init__(self, master: app.App):
        super().__init__(master)

        self.app = master

        self.play_button = tk.Checkbutton(
            self, text="Play", variable=self.app.state.playing, command=self.on_play)
        self.record_button = tk.Checkbutton(
            self, text="Record", variable=self.app.state.recording, command=self.on_record)

        self.play_button.pack(side=tk.LEFT)
        self.record_button.pack(side=tk.LEFT)

    def on_play(self, *args):
        print(self.app.state.playing.get())

    def on_record(self, *args):
        if self.app.state.recording.get():
            hardware.listener.start(self.set_text)
        else:
            hardware.listener.stop()

    def set_text(self, text: str):
        old_text = self.app.state.instructions_text.get()
        self.app.state.instructions_text.set((old_text + '\n' + text).strip())
