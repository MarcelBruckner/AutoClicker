
from typing import Callable
import hardware.my_mouse as my_mouse
import hardware.my_keyboard as my_keyboard


def start(target_func: Callable[[str], None]):
    my_mouse.start_listen(target_func)
    my_keyboard.start_listen(target_func)


def stop():
    my_mouse.stop_listen()
    my_keyboard.stop_listen()
