from typing import Union
from PIL import Image
from enum import Enum
import numpy as np
from PIL import ImageTk as itk

from flaskr.util import Size


class ImageType(Enum):
    """Enum type for the supported image conversions

    Args:
        Enum (enum.Enum): Base enum class
    """
    PIL = 0
    OPENCV = 1
    TK = 2


def convert_pil_image(img: Image.Image, image_type: ImageType) -> Union[Image.Image, np.array, itk.PhotoImage]:
    """Converts the given PIL image to the desired format.

    Args:
        img (Image.Image): The given image.
        image_type (ImageType): The desired image format.

    Raises:
        ValueError: Raised if an unknown image_type is given.

    Returns:
        Union[Image.Image, np.array, itk.PhotoImage]: Depending on the image_type.
    """
    match image_type:
        case ImageType.PIL:
            return img
        case ImageType.OPENCV:
            return np.array(img)[:, :, ::-1].copy()
        case ImageType.TK:
            return itk.PhotoImage(img)
        case _:
            raise ValueError('Invalid image_type given')


def resize(img: Image, size: Size = None, width: int = 512, height: int = 512):
    """Replaces the image IN-PLACE to fit within the size bounding box. 
    Keeps the aspect ratio.

    Args:
        img (Image): The image to resize
        size (Size, optional): The given maximal size of the image. Defaults to None.
    """
    if not size:
        size = Size(width, height)
    img.thumbnail((size.width, size.height), Image.Resampling.LANCZOS)
