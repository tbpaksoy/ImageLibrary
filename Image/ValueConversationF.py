def isHex(source):
    if type(source) is str:
        for i in source:
            if not i in "0123456789ABCDEFabcdef":
                return False
        return True
    else:
        return False


def isDecimal(source):
    if type(source) is int:
        return True
    elif type(source) is str:
        for i in source:
            if not i in "0123456789":
                return False
        return True
    else:
        return False


def toDecimal(source):
    for i in source:
        if not isHex(i):
            return
    result = []
    for i in source:
        result.append(int(i, 16))
    return result


def toHex(source, length):
    if isDecimal(source):
        temp = 0
        if type(source) is str:
            temp = int(source)
        temp = source
        result = hex(temp)
        result = result[2:len(result)]
        while len(result) < length:
            result = "0" + result
        return result


def fromHexToBinaryList(source: list):
    result = []
    for i in source:
        temp = bin(int(i,16))[2:]
        result.append(temp)
    return result


def fromHexToDecimalList(source: list):
    result = []
    for i in source:
        result.append(int(i, 16))
    return result

def fromStrToDecimalList(source: list):
    result = []
    for i in source:
        result.append(int(i))
    return result
def fromHexToByteArray(source:list):
    result = []
    for i in source:
        result.append(int(i,16))
    return bytearray(result)
    