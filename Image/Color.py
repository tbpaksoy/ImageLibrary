from dataclasses import dataclass, field
import os
import json


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

    def __init__(self, hex: str):
        self.r = self.g = self.b = 0
        self.a = 255
        for c in hex:
            if not c in "0123456789ABCDEFabcdef":
                return
        if len(hex) == 6 or len(hex) == 8:
            self.r = int(hex[0: 2], 16)
            self.g = int(hex[2: 4], 16)
            self.b = int(hex[4: 6], 16)
            if len(hex) == 8:
                self.a = int(hex[6: 8], 16)

    def printValues(self):
        print(str(self.r)+","+str(self.g)+","+str(self.b)+","+str(self.a))

    def getNegative(self):
        return Color(255-self.r, 255-self.g, 255-self.b, 255-self.a)

    def createColorPalette(colors: list, width: int, height: int) -> list:
        palette = []
        for i in range(0, width):
            temp = []
            for j in range(0, height):
                if i*j+j < len(colors):
                    temp.append(colors[i*j+j])
                else:
                    temp.append(Color("00000000"))
            palette.append(temp)
        return palette

    def scalePalette(palette: list, size: int) -> list:
        result = []
        for i in range(len(palette[0])*size):
            temp = []
            for j in range(len(palette)*size):
                temp.append(palette[int(i/size)][int(j/size)])
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
                return Color(data[name])
    return Color("FFFFFFFF")


print(getColorFromLibrary("red"))
