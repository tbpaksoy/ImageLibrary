from dataclasses import dataclass
import os
import json
import MathF


@dataclass
class Color:
    r: int
    g: int
    b: int
    a: int

    def __str__(self) -> str:
        return "Col: (" + str(self.r) + "," + str(self.g) + "," + str(self.b) + "," + str(self.a) + ")"

    def __init__(self, r: int, g: int, b: int, a: int):
        self.r = r
        self.g = g
        self.b = b
        self.a = a

    def fromHex(hex: str):
        r = g = b = 0
        a = 255
        for c in hex:
            if not c in "0123456789ABCDEFabcdef":
                return
        if len(hex) == 6 or len(hex) == 8:
            r = int(hex[0: 2], 16)
            g = int(hex[2: 4], 16)
            b = int(hex[4: 6], 16)
            if len(hex) == 8:
                a = int(hex[6: 8], 16)
        return Color(r, g, b, a)

    def printValues(self):
        print(str(self.r)+","+str(self.g)+","+str(self.b)+","+str(self.a))

    def getNegative(self):
        return Color(255-self.r, 255-self.g, 255-self.b, 255-self.a)

    def createColorPalette(colors: list, width: int, height: int) -> list:
        palette = []
        for i in range(0, width):
            temp = []
            for j in range(0, height):
                if j*height+i < len(colors):
                    temp.append(colors[height*j+i])
                else:
                    temp.append(Color(0,0,0,1))
            palette.append(temp)
        return palette

    def createColorTransition(a: list, b: list, step: int):
        step = max(0, step)
        while len(a) < max(len(a), len(b)):
            a.append(Color())
        while len(b) < max(len(a), len(b)):
            b.append(Color())
        palette = []
        for i in range(len(a)):
            temp = []
            temp.append(a[i])
            for j in range(1, step-1):
                r = int(a[i].r + MathF.goToValue(a[i].r, b[i].r) * j / i)
                g = int(a[i].g + MathF.goToValue(a[i].g, b[i].g) * j / i)
                b = int(a[i].b + MathF.goToValue(a[i].b, b[i].b) * j / i)
                temp.append(Color(r, g, b, 1))
            temp.append(b[i])
            palette.append(temp)
        return palette

    def scalePalette(palette: list, size: int) -> list:
        result = []
        for i in range(len(palette[0])*size):
            temp = []
            for j in range(len(palette)*size):
                    temp.append(palette[j//size][i//size])
                #print(str(i // size) + " " + str(j // size))
            result.append(temp)
        return result


def toSingleList(source: list):
    result = []
    for i in source:
        for j in i:
            result.append(j)
    return result


def getColorFromLibrary(name: str):
    for file in os.listdir("C:\\Users\\Tahsin\\Desktop\\Image\\Colors\\"):
        if file[len(file)-5:] == ".json":
            temp = open("C:\\Users\\Tahsin\\Desktop\\Image\\Colors\\" + file)
            data = json.load(temp)
            if name in data:
                return Color(data[name]["r"], data[name]["g"], data[name]["b"], data[name]["a"])
    return Color.fromHex("FFFFFFFF")
def getColorsFromCollection(name: str):
    result = []
    for file in os.listdir("C:\\Users\\Tahsin\\Desktop\\Image\\Colors"):
        if file[len(file)-5:] == ".json" and file == name + ".json":
            temp = open("C:\\Users\\Tahsin\\Desktop\\Image\\Colors\\" + file)
            data = json.load(temp)
            for color in data:
                result.append(Color(data[color]["r"], data[color]["g"], data[color]["b"], data[color]["a"]))
    return result