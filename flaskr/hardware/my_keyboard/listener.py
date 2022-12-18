from time import sleep
from typing import Callable
from pynput import keyboard

pressed_keys = set()


def on_press(key):
    global pressed_keys

    try:
        pressed_keys.add(key.char)
    except AttributeError:
        pressed_keys.add(key)


def on_release(key):
    global pressed_keys

    try:
        pressed_keys.remove(key.char)
    except AttributeError:
        pressed_keys.remove(key)
    _target_func(f'Released: {key}')


def get_listener():
    return keyboard.Listener(on_press=on_press,
                             on_release=on_release)


_listener: keyboard.Listener = None
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
    _listener = get_listener()
    _listener.start()
    sleep(5)
    _listener.stop()
