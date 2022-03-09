
from xmlrpc.client import MAXINT
from ArrayF import reverseGroup,reverseArray
from ValueConversationF import toHex, fromHexToByteArray
import Color
import os
import time


def createBMPHeader(width: int, height: int):
    data = []
    data.append("42")
    data.append("4D")
    paddingCount = width * 3
    while not paddingCount % 4 == 0:
        paddingCount += 1
    for i in reverseGroup(toHex(54 + width * height * 3 + height * paddingCount, 8).upper(), 2):
        data.append(i)
    for i in range(4):
        data.append("00")
    for i in reverseGroup(toHex(54, 8).upper(), 2):
        data.append(i)
    for i in reverseGroup(toHex(40, 8).upper(), 2):
        data.append(i)
    for i in reverseGroup(toHex(width, 8).upper(), 2):
        data.append(i)
    for i in reverseGroup(toHex(height, 8).upper(), 2):
        data.append(i)
    data.append("01")
    data.append("00")
    data.append("18")
    data.append("00")
    for i in range(4):
        data.append("00")
    for i in reverseGroup(toHex(16, 8).upper(), 2):
        data.append(i)
    for j in range(2):
        for i in reverseGroup(toHex(2835, 8).upper(), 2):
            data.append(i)
    for i in range(8):
        data.append("00")

    return data


def generateColorPallette(width: int, height: int, size: int, source: list):
    if width * height < len(source):
        pass
    else:
        colors = Color.Color.scalePalette(
            Color.Color.createColorPalette(source, width, height), size)
        data = []
        for i in createBMPHeader(width*size, height*size):
            data.append(i)
        for i in generateColorColorMatrix(width, height, colors):
            data.append(i)
    return data


def generateColorTransition(a: list, b: list, step: int, size: int, default=Color.Color(255, 255, 255, 255)) -> list:
    colors = Color.Color.scalePalette(
        Color.generateColorTransition(a, b, step, default), size)
    data = []
    for i in createBMPHeader(len(a)*size, (step+2) * size):
        data.append(i)
    for i in generateColorColorMatrix(len(colors), step+2, colors):
        data.append(i)
    return data


def generateColorColorMatrix(width, height, source: list):
    result = []
    for subSource in source:
        for color in subSource:
            result.append(toHex(color.b, 2).upper())
            result.append(toHex(color.g, 2).upper())
            result.append(toHex(color.r, 2).upper())
        while len(result) % 4 != 0:
            result.append("00")

    return result


a = Color.getColorsFromCollection("Authentic")
b = reverseArray(a)

palette = generateColorTransition(a, b, 10, 25)
"""
for i in range(len(palette)):
    if len(palette[i]) > 2:
        print(palette[i],i,sep = ":")
"""
file = open("try.bmp", "wb")
file.write(fromHexToByteArray(palette))
file.close()
