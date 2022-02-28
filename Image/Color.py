from dataclasses import dataclass
@dataclass
class Color:
    r: float
    g: float
    b: float
    a: float

    def __init__(self, r, g, b, a):
        self.r = r
        self.g = g
        self.b = b
        self.a = a

    def __init__(self, hex: str):
        self.r = self.g = self.b = 0.0
        self.a = 1.0
        for c in hex:
            if not c in "0123456789ABCDEFabcdef":
                return
        if len(hex) == 6 or len(hex) == 8:
            self.r = float(int(hex[0: 2], 16)) / 255.0
            self.g = float(int(hex[2: 4], 16)) / 255.0
            self.b = float(int(hex[4: 6], 16)) / 255.0
            if len(hex) == 8:
                self.a = float(int(hex[6: 8], 16)) / 255.0
    def printValues(self):
        print(str(self.r)+","+str(self.g)+","+str(self.b)+","+str(self.a))

    def getNegative(self):
        return Color(1.0-self.r, 1.0-self.g, 1.0-self.b, 1.0-self.a)