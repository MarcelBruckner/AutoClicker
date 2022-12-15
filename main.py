from capture_util import *
from image_util import ImageType, convert_pil_image, resize
from util import timeit
from widgets.app import App, Size
import cv2
from threading import Thread

hwnd = get_hwnd(window_title='RuneScape')

canvas_size=Size(width=330, height=330)
window = App(text_box_size=Size(width=50, height=20))
cv2.namedWindow("Preview", cv2.WINDOW_KEEPRATIO)

@timeit
def mainloop():
    image = grab_window_content(hwnd=hwnd)
    
    cv2_image = convert_pil_image(img=image, image_type=ImageType.OPENCV)
    cv2.imshow("Preview", cv2_image)
    cv2.waitKey(1)

    window.update()

while not window.is_closed:
    mainloop()