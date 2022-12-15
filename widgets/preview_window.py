import tkinter as tk
from util import Size
import widgets
from image_util import convert_pil_image, ImageType, resize


class PreviewWindow(tk.Toplevel):

    def __init__(self, master: widgets.App):
        super().__init__(master)
        self.width = 512
        self.height = 512
        self.canvas = tk.Canvas(self, width=self.width, height=self.height)
        self.canvas.pack()
        self.canvas_image = self.canvas.create_image(
            self.width/2, self.height/2, anchor=tk.CENTER, image=None)

        self.app = master
        self.protocol("WM_DELETE_WINDOW", self.close_window)

    def close_window(self):
        self.app.state.render_preview.set(False)

    def update(self, image):
        try:
            resize(image, Size(width=self.width, height=self.height))
            image = convert_pil_image(image, ImageType.TK)
            self.canvas.imgref = image
            self.canvas.itemconfig(self.canvas_image, image=image)
            super().update()
        except:
            pass
