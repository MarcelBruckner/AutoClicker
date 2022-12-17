from time import sleep
from pynput import keyboard


def on_press(key):
    try:
        print('alphanumeric key {0} pressed'.format(
            key.char))
    except AttributeError:
        print('special key {0} pressed'.format(
            key))


def on_release(key):
    print('{0} released'.format(
        key))


def get_listener():
    return keyboard.Listener(on_press=on_press,
                             on_release=on_release)


listener: keyboard.Listener = None


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
