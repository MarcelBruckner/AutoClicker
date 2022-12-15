from dataclasses import dataclass
from functools import wraps
import time

@dataclass
class Size:
    """Helper class for size based on width and height.
    """
    height: int
    width: int


def timeit(func):
    """Decorator to measure the timing of a function.

    Args:
        func: The function to measure.

    Returns:
        func: The wrapped function.
    """
    @wraps(func)
    def timeit_wrapper(*args, **kwargs):
        start_time = time.perf_counter()
        result = func(*args, **kwargs)
        end_time = time.perf_counter()
        total_time = end_time - start_time
        print(f'{func.__name__}{args} {kwargs}: {total_time:.4f} s / {1.0 / total_time:.1f} fps')
        return result
        
    return timeit_wrapper