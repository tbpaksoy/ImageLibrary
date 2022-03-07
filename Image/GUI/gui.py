from kivy.app import App
from kivy.uix.button import Button
from kivy.uix.label import Label
from kivy.uix.gridlayout import GridLayout
from kivy.uix.floatlayout import FloatLayout

class TestApp(App):
    def build(self):
        mainlo = GridLayout(cols = 16, rows = 20)
        lo1 = FloatLayout()
        lo2 = FloatLayout()
        lo1.add_widget(Button(text = "Paluk"))
        mainlo.add_widget(lo1)
        lo2.add_widget(Button(text = "Kupa"))
        mainlo.add_widget(lo2)
        return mainlo

    def changeColor(self,button: Button):
        button.background_color = (.5, .5, .5, 1)
        self.color = (1,1,1,1)

    def changeCol(label: Label):
        label.color = (1,0,0,1)
        


TestApp().run()
