from time import sleep
import time
from typing import Callable
import numpy as np
from pynput import mouse


pressed_button = None
down_position = (-1, -1)
up_position = (-1, -1)
current_position = (-1, -1)
last_scroll = -1


def on_move(x, y):
    global current_position
    current_position = (x, y)


def on_click(x, y, button, pressed):
    global down_position
    global up_position
    global pressed_button

    pressed_button = button if pressed else None
    down_position = (x, y) if pressed else down_position
    up_position = (x, y) if not pressed else down_position

    if not pressed:
        if np.linalg.norm(np.array(down_position) - np.array(up_position)) > 10:
            _target_func(
                f"Dragged: {button} from {down_position} to {up_position}")
        else:
            _target_func(f"Clicked: {button} at {up_position}")


def on_scroll(x, y, dx, dy):
    global last_scroll

    scroll = time.time()
    if scroll - last_scroll > 200:
        last_scroll = scroll
        _target_func(f"Scroll: {'down' if dy < 0 else 'up'} at {x, y}")


def get_listener() -> mouse.Listener:
    return mouse.Listener(on_move=on_move,
                          on_click=on_click,
                          on_scroll=on_scroll)


_listener: mouse.Listener = None
_target_func: Callable[[str], None] = None


def start(target_func: Callable[[str], None]):
    global _listener
    global _target_func

    _target_func = target_func

    if not _listener:
        _listener = get_listener()
        _listener.start()


def stop():
    global _listener

    if _listener:
        _listener.stop()
        _listener = None


if __name__ == "__main__":
    start(lambda x: print(x))

    _listener = get_listener()
    _listener.start()
    sleep(5)
    _listener.stop()
