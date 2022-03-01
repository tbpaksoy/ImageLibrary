from ssl import DefaultVerifyPaths
from ArrayF import reverseGroup
from ValueConversationF import toHex
def createBMPHeader(width=0, height=0):
    data = []
    data.append("42")
    data.append("4D")
    paddingCount = width * 3 % 4
    for i in reverseGroup(toHex(54 + width * height * 3 + height * paddingCount,8),2):
        data.append(i)
    for i in range(4):
        data.append("00")
    for i in reverseGroup(toHex(54,8),2):
        data.append(i)
    for i in reverseGroup(toHex(40,8),2):
        data.append(i)
    for i in reverseGroup(toHex(width,8),2):
        data.append(i)
    for i in reverseGroup(toHex(height,8),2):
        data.append(i)
    data.append("01")
    data.append("00")
    data.append("18")
    data.append("00")
    for i in range(4):
        data.append("00")
    for i in reverseGroup(toHex(16,8),2):
        data.append(i)
    for j in range(2):
        for i in reverseGroup(toHex(2835,8),2):
            data.append(i)
    for i in range(8):
        data.append("00")
    
    return data

for i in createBMPHeader(2,2):
    print(i)
