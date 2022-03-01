def groupArray(source, length):
    result = []
    if len(source) % length != 0:
        return result
    for i in range(int(len(source)/length)):
        result.append(source[i*length:i*length+length])
    return result
def reverseArray(source):
    result = []
    for i in range(len(source)):
        result.append(source[len(source)-i-1])
    return result
def reverseGroup(source,length):
    result = groupArray(source,length)
    result = reverseArray(result)
    return result