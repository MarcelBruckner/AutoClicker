from time import sleep
from pynput import mouse


pressed_button = None
down_position = (-1, -1)
up_position = (-1, -1)


def on_move(x, y):
    global down_position
    global up_position
    global pressed_button

    if pressed_button:
        print(f'{pressed_button}: Pointer dragged from {down_position} to {x, y}')
    else:
        print(f'Pointer moved to {x, y}')


def on_click(x, y, button, pressed):
    global down_position
    global up_position
    global pressed_button

    pressed_button = button if pressed else None
    down_position = (x, y) if pressed else down_position
    up_position = (x, y) if not pressed else down_position

    print(f"{button}: {'Pressed' if pressed else 'Released'} at {down_position if pressed else up_position}")


def on_scroll(x, y, dx, dy):

    print(f"Scrolled {'down' if dy < 0 else 'up'} at {x, y}")


def get_listener() -> mouse.Listener:
    return mouse.Listener(on_move=on_move,
                          on_click=on_click,
                          on_scroll=on_scroll)


listener: mouse.Listener = None


def listen():
    global listener

    if not listener:
        listener = get_listener()
        listener.start()


def stop_listen():
    global listener

    if listener:
        listener.stop()
        listener = None


if __name__ == "__main__":
    listener = get_listener()
    listener.start()
    sleep(5)
    listener.stop()
