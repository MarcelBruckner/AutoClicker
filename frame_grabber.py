import subprocess
from time import sleep
from mss import mss
from PIL import Image
import os
from pathlib import Path

rs = "RuneScape"

def search_windows_by_name(name):
    try:
        return subprocess.check_output(["xdotool", "search", "--name", name]).splitlines()
    except:
        return []
    
def activate_window(window):
    if window:
        return subprocess.call(["xdotool", "windowactivate", window])

def get_window_geometry(window):
    if not window:
        return ((10,10),(10,10))
    
    stats = subprocess.check_output(["xdotool", "getwindowgeometry", window]).splitlines()
    geometry = 0
    position = 0
    for _stat in stats:
        stat = _stat.decode("utf-8").strip()
        if stat.startswith("Position: "):
            stat = stat[stat.find(" "):].strip()
            stat = stat[:stat.find(" ")].strip()
            position = stat.split(",")
            position = (int(position[0]), int(position[1]))
        if stat.startswith("Geometry: "):
            stat = stat[stat.find(" "):].strip()
            geometry = stat.split("x")
            geometry = (int(geometry[0]), int(geometry[1]))

    return (position, geometry)     

def get_game_window():
    for window in search_windows_by_name(rs):
        if get_window_geometry(window) != ((10,10),(10,10)):
            return window
    return None

def get_window_bbox(window):
    if not window:
        return None
    geometry = get_window_geometry(window)
    left = geometry[0][0]
    top = geometry[0][1] - 19
    right = left + geometry[1][0]
    bottom = top + geometry[1][1]
    return (left, top, right, bottom)    
    
def get_window_viewport(window):
    bbox = get_window_bbox(window)
    if not bbox:
        return None
    
    with mss() as sct:
        monitor = sct.monitors[1]
        sct_img = sct.grab(bbox)
        return Image.frombytes('RGB', sct_img.size, sct_img.bgra, 'raw', 'BGRX')

def make_square(image):
    desired_size = 1024
    old_size = image.size
    
    ratio = float(desired_size)/max(old_size)
    new_size = tuple([int(x*ratio) for x in old_size])
    
    image = image.resize(new_size, Image.ANTIALIAS)
    # create a new image and paste the resized on it

    new_im = Image.new("RGB", (desired_size, desired_size))
    new_im.paste(image, ((desired_size-new_size[0])//2,
                        (desired_size-new_size[1])//2))
    return new_im
    
def get_current_frame():
    game_window = get_game_window()
    if not game_window:
        return None
    
    activate_window(game_window)
    return make_square(get_window_viewport(game_window))

def record_game(path):
    if not os.path.exists(path):
        os.makedirs(path)

    i = 0
    while True:
        im = get_current_frame()
        while Path(path + f'sample_{i}.png').exists():
            i += 1
        im.save(path + f'sample_{i}.png')
        i += 1