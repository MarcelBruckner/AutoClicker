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


if __name__ == "__main__":
    listener = get_listener()
    listener.start()
    sleep(5)
    listener.stop()
