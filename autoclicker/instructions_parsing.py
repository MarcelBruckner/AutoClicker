'''
https://github.com/lark-parser/lark
https://github.com/lark-parser/lark/blob/master/lark/grammars/common.lark
'''
import io
from pprint import pprint
from lark import Lark, tree, Transformer
import os
from PIL import Image
import lark
os.environ["PATH"] += os.pathsep + 'C:\\Program Files\\Graphviz\\bin'


def make_png(parsed: lark.ParseTree, filename: str = 'autoclicker/parsed.png'):
    tree.pydot__tree_to_png(parsed, filename)


def make_dot(parsed: lark.ParseTree, filename: str = 'autoclicker/parsed.dot'):
    tree.pydot__tree_to_dot(parsed, filename)


def make_graph(parsed: lark.ParseTree):
    graph = tree.pydot__tree_to_graph(parsed)
    return graph


def show_graph(parsed: lark.ParseTree):
    graph = make_graph(parsed)
    png = graph.create_png()
    image = Image.open(io.BytesIO(png))
    image.show()


def parse(instructions: str) -> lark.ParseTree:
    return parser.parse(instructions)


class MyTransformer(Transformer):
    list = list

    def instructions(self, args):
        return args

    def click(self, args):
        return args

    def keyboard(self, args):
        return args

    def keys(self, value):
        (value, ) = value
        if isinstance(value, list):
            return value
        return str(value)

    def x(self, value):
        (value, ) = value
        return float(value)

    def y(self, value):
        (value, ) = value
        return float(value)

    def button(self, value):
        (value, ) = value
        return str(value)

    def char(self, value):
        (value, ) = value
        return str(value)

    def number(self, value):
        (value, ) = value
        return float(value)

    def string(self, value):
        (value, ) = value
        return str(value)


if __name__ == "__main__":
    parser = Lark.open('autoclicker/grammar.lark', start='instructions')

    instructions = '''
                Click:    x=3 y=5 button=left
                Keyboard: keys=Z
                Keyboard: keys=[a, b, c]
                '''

    parsed = parse(instructions)
    print(parsed.pretty())
    # show_graph(parsed)

    transformed = MyTransformer().transform(parsed)
    pprint(transformed)
