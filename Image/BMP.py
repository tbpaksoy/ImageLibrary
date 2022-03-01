from ArrayF import reverseGroup
from ValueConversationF import toHex
from Color import Color

def createBMPHeader(width=0, height=0):
    data = []
    data.append("42")
    data.append("4D")
    paddingCount = width * 3 % 4
    for i in reverseGroup(toHex(54 + width * height * 3 + height * paddingCount, 8), 2):
        data.append(i)
    for i in range(4):
        data.append("00")
    for i in reverseGroup(toHex(54, 8), 2):
        data.append(i)
    for i in reverseGroup(toHex(40, 8), 2):
        data.append(i)
    for i in reverseGroup(toHex(width, 8), 2):
        data.append(i)
    for i in reverseGroup(toHex(height, 8), 2):
        data.append(i)
    data.append("01")
    data.append("00")
    data.append("18")
    data.append("00")
    for i in range(4):
        data.append("00")
    for i in reverseGroup(toHex(16, 8), 2):
        data.append(i)
    for j in range(2):
        for i in reverseGroup(toHex(2835, 8), 2):
            data.append(i)
    for i in range(8):
        data.append("00")

    return data


def generateColorColorMatrix(width, height, source):
    paddingCount = width * 3 % 4
    result = []
    for i in range(width):
        for j in range(height):
            color = Color(source[i*j+j])
            result.append(toHex(color.r * 255))
            result.append(toHex(color.g * 255))
            result.append(toHex(color.b * 255))
        for k in range(paddingCount):
            result.append("00")

    return result

list = [Color("FFFFFF"),Color("FFFFFF"),Color("FFFFFF"),Color("FFFFFF")]
for i in generateColorColorMatrix(2,2,list):
    print(i)
