from dataclasses import dataclass
from enum import Enum
import math
from random import random
from typing import Callable, List, Union
import pyautogui


def _random_direction():
    angle = random() * 2 * math.pi
    return math.sin(angle), math.cos(angle)


def _randomize_position(x: float, y: float, displacement: int = 5):
    direction = _random_direction()
    length = random() * displacement
    xx, yy = (d * length for d in direction)
    return x + xx, y + yy


def _randomize_duration(duration: float, displacement: float = 1.):
    return max(0.1, duration + random() * displacement)


def click(x: float, y: float, position_displacement: float = 5,
          button: Union[pyautogui.LEFT, pyautogui.MIDDLE,
                        pyautogui.RIGHT] = pyautogui.LEFT,
          duration: float = 0.5, duration_displacement: float = 0.5,
          tween: Callable[[float], float] = pyautogui.easeOutElastic):
    pyautogui.click(
        *_randomize_position(x=x, y=y, displacement=position_displacement),
        button=button,
        duration=_randomize_duration(
            duration=duration, displacement=duration_displacement),
        tween=tween
    )


if __name__ == "__main__":
    pass
