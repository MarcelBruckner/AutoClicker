from capture_util import *
from image_util import ImageType, convert_pil_image, resize
from util import timeit
from widgets.app import App, Size
from threading import Thread

if __name__=="__main__":
    window = App(text_box_size=Size(width=50, height=20))

    while not window.is_closed:
        window.update()