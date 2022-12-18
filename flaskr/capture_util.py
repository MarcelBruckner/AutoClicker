from time import time
from greenlet import getcurrent as get_ident
import logging
from flaskr.util import Size
from flaskr.image_util import ImageType, convert_pil_image, resize
import threading
from typing import List, Tuple, Union
import win32gui
import ctypes
from PIL import ImageGrab, Image
import cv2
from screeninfo import get_monitors
import ctypes
ctypes.windll.user32.SetProcessDPIAware()


def get_all_window_handles() -> List:
    """Gets all window handles.

    Returns:
        Tuple[List, List]: The toplist and winlist
    """
    toplist, winlist = [], []

    def enum_cb(hwnd, results):
        if win32gui.IsWindowVisible(hwnd):
            winlist.append((hwnd, win32gui.GetWindowText(hwnd)))
    win32gui.EnumWindows(enum_cb, toplist)

    return winlist


def get_all_window_titles(unique=True, _sorted=True):
    winlist = [x[1] for x in get_all_window_handles()]
    if unique:
        winlist = list(filter(lambda x: x, list(set(winlist))))
    if _sorted:
        winlist = sorted(winlist, key=str.lower)
    return winlist


def get_hwnd(window_title: str) -> int:
    """Gets the window handle of the window with the given title.

    Args:
        window_title (str): The desired window title (case-insensitive).

    Raises:
        ValueError: Raised if there is no window with the given title.

    Returns:
        int: The window handle.
    """
    winlist = get_all_window_handles()
    hwnds = [(hwnd, title)
             for hwnd, title in winlist if window_title.lower() in title.lower()]

    for hwnd in hwnds:
        try:
            win32gui.SetForegroundWindow(hwnd[0])
            return hwnd[0]
        except:
            pass

    raise ValueError(f'Cannot find a window with title {window_title}')


def get_bbox(hwnd: int) -> Tuple[float, float, float, float]:
    """Calculates the bounding box of the window.

    Args:
        hwnd (int): The window handle.

    Returns:
        Tuple[float, float,float,float]: x, y, width, height of the window.
    """
    rect = ctypes.wintypes.RECT()
    ctypes.windll.user32.GetWindowRect(hwnd, ctypes.pointer(rect))
    bbox = (rect.left, rect.top, rect.right, rect.bottom)

    if sum(bbox) == 0:
        raise ValueError(f'Cannot get bbox for hwnd: {hwnd}')
    return bbox


def grab_window_content(hwnd: int) -> Union[Image.Image, None]:
    """Grabs the content of the hwnd.
    If the window is occluded, the overlayed windows are captured. 

    Args:
        hwnd (int): The window handle.

    Returns:
        Union[PIL.Image.Image, None]: The image if a capture was taken, None if there happenes an exception.
    """
    bbox = get_bbox(hwnd=hwnd)
    image = ImageGrab.grab(bbox)
    return image


capture_thread: threading.Thread = None
capture_running = False
image = None


def get_image() -> Image.Image:
    return image


def capture_loop(hwnd: int):
    global capture_running
    global image

    try:
        win32gui.SetForegroundWindow(hwnd)
        capture_running = True
        while capture_running:
            image = grab_window_content(hwnd=hwnd)
    except Exception as e:
        logging.warning(e)
        stop_capture()


class CameraEvent(object):
    """An Event-like class that signals all active clients when a new frame is
    available.
    """

    def __init__(self):
        self.events = {}

    def wait(self):
        """Invoked from each client's thread to wait for the next frame."""
        ident = get_ident()
        if ident not in self.events:
            # this is a new client
            # add an entry for it in the self.events dict
            # each entry has two elements, a threading.Event() and a timestamp
            self.events[ident] = [threading.Event(), time.time()]
        return self.events[ident][0].wait()

    def set(self):
        """Invoked by the camera thread when a new frame is available."""
        now = time()
        remove = None
        for ident, event in self.events.items():
            if not event[0].isSet():
                # if this client's event is not set, then set it
                # also update the last set timestamp to now
                event[0].set()
                event[1] = now
            else:
                # if the client's event is already set, it means the client
                # did not process a previous frame
                # if the event stays set for more than 5 seconds, then assume
                # the client is gone and remove it
                if now - event[1] > 5:
                    remove = ident
        if remove:
            del self.events[remove]

    def clear(self):
        """Invoked from each client's thread after a frame was processed."""
        self.events[get_ident()][0].clear()


def start_capture(title: str):
    global capture_running
    global capture_thread

    if capture_running:
        return

    hwnd = None
    for _ in range(100):
        try:
            hwnd = get_hwnd(title)
            break
        except:
            pass
    else:
        return False

    capture_thread = threading.Thread(target=capture_loop, args=(hwnd,))
    capture_thread.start()

    return capture_running


def stop_capture():
    global capture_running
    global capture_thread

    capture_running = False

    if capture_thread:
        capture_thread = None


if __name__ == "__main__":
    for monitor in get_monitors():
        print(monitor)
