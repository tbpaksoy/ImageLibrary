import random
from xmlrpc.client import MAXINT
from ArrayF import reverseGroup
from ValueConversationF import fromHexToBinaryList, fromHexToByteArray, fromHexToDecimalList, fromStrToDecimalList, toHex
import Color
from Color import toSingleList
import os
import time


def createBMPHeader(width: int, height: int):
    data = []
    data.append("42")
    data.append("4D")
    paddingCount = width * 3
    while not paddingCount % 4 == 0:
        paddingCount += 1
    paddingCount %= 4
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


def generateColorColorMatrix(width, height, source: list):
    result = []
    for subSource in source:
        for color in subSource:
            result.append(toHex(color.b, 2).upper())
            result.append(toHex(color.g, 2).upper())
            result.append(toHex(color.r, 2).upper())
        while not len(result) % 4 == 0:
            result.append("00")

    return result

for col in os.listdir(os.getcwd+"\\Colors"):
    palette = Color.getColorsFromCollection(col)
        
