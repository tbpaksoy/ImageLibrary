from kivy.app import App
from kivy.uix.button import Button
from kivy.uix.label import Label
from kivy.uix.boxlayout import BoxLayout
from kivy.app import App
from kivy.uix.boxlayout import BoxLayout
from kivy.graphics import Color, Rectangle
from kivy.uix.colorpicker import ColorPicker
from kivy.core.window import Window
from kivy.lang import Builder

class TestApp(App):
    def build(self):
        mainlo = BoxLayout()
        self._app_name = "Tahsin"
        self.title = "Tahsin"
        lo = BoxLayout()
        label = Label(text = "asd", width = 10, height = 20,font_size = 100)
        lo.add_widget(label)
        button = Button(text="asd")
        button.on_press = lambda *t: (self.changeColor(button=button),
                                      TestApp.changeCol(label=label))
        lo.add_widget(button)
        Window.clearcolor = (1,1,1,1)
        mainlo.add_widget(lo)
        mainlo.add_widget(ColorPicker())
        return mainlo

    def changeColor(self,button: Button):
        button.background_color = (.5, .5, .5, 1)
        self.color = (1,1,1,1)

    def changeCol(label: Label):
        label.color = (1,0,0,1)
        


TestApp().run()