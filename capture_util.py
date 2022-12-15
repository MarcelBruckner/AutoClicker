from typing import List, Tuple, Union
import PIL
import win32gui
import ctypes
from PIL import ImageGrab, Image

def get_dpi() -> float:
    """Calculates the display resolution.

    Returns:
        float: The resolution.
    """
    hwnd = win32gui.GetDesktopWindow()
    ratio = 1.0
    try:
        dpi = ctypes.windll.user32.GetDpiForWindow(hwnd)
        ratio = dpi / 96.0
    except:
        pass
    return ratio


def get_all_window_handles() -> Tuple[List, List]:
    """Gets all window handles.

    Returns:
        Tuple[List, List]: The toplist and winlist
    """
    toplist, winlist = [], []

    def enum_cb(hwnd, results):
        winlist.append((hwnd, win32gui.GetWindowText(hwnd)))
    win32gui.EnumWindows(enum_cb, toplist)
    return toplist, winlist


def get_hwnd(window_title: str) -> int:
    """Gets the window handle of the window with the given title.

    Args:
        window_title (str): The desired window title (case-insensitive).

    Raises:
        ValueError: Raised if there is no window with the given title.

    Returns:
        int: The window handle.
    """
    _, winlist = get_all_window_handles()
    hwnds = [(hwnd, title)
             for hwnd, title in winlist if window_title.lower() in title.lower()]

    for hwnd in hwnds:
        try:
            win32gui.SetForegroundWindow(hwnd[0])
            return hwnd[0]
        except:
            pass

    raise ValueError(f'Cannot find a window with title {window_title}')


def get_bbox(hwnd: int) -> Tuple[float, float,float,float]:
    """Calculates the bounding box of the window.

    Args:
        hwnd (int): The window handle.

    Returns:
        Tuple[float, float,float,float]: x, y, width, height of the window.
    """
    bbox = win32gui.GetWindowRect(hwnd)
    scale = get_dpi()
    return (b * scale for b in bbox)


def grab_window_content(hwnd: int) -> Union[Image.Image, None]:
    """Grabs the content of the hwnd.
    If the window is occluded, the overlayed windows are captured. 

    Args:
        hwnd (int): The window handle.

    Returns:
        Union[PIL.Image.Image, None]: The image if a capture was taken, None if there happenes an exception.
    """
    try:
        # win32gui.SetForegroundWindow(hwnd)
        bbox = get_bbox(hwnd=hwnd)
        image = ImageGrab.grab(bbox)
        return image
    except:
        return None
