from Color import getColorsFromCollection
from kivy.app import App
from kivy.uix.button import Button
from kivy.uix.label import Label
from kivy.uix.gridlayout import GridLayout
from kivy.uix.textinput import TextInput
from BMP import generateColorTransition
from ValueConversationF import fromHexToByteArray
from ArrayF import reverseArray
from kivy.uix.floatlayout import FloatLayout
from kivy.uix.boxlayout import BoxLayout
from kivy.uix.slider import Slider

class TestApp(App):
    def build(self):
        lo = GridLayout()
        for i in range(4):
            sb = TestApp.getSubColorLayout()
            lo.add_widget(sb)
            sb.center= (i*lo.center_x*2+lo.center_x/2,i*lo.center_y*2+lo.center_y/2)
        return lo
    def exportColor(colorName:str,exportName:str = "unnamed"):
        if not exportName:
            exportName = "unnamed"
        col = getColorsFromCollection(colorName)
        rev = reverseArray(col)
        palette = generateColorTransition(col,rev,7,25)
        file = open(exportName + ".bmp", "wb")
        file.write(fromHexToByteArray(palette))
        file.close()
    def getSubColorLayout():
        lo = BoxLayout()
        lo.orientation = 'vertical'
        for i in range(4):
            lo.add_widget(Slider())
        return lo







TestApp().run()
