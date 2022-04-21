from typing import Callable
import dearpygui.dearpygui as dpg


dpg.create_context()
dpg.create_viewport()
dpg.setup_dearpygui()

cw = 150
ch = 150
spacing = 75
chcp = 0


with dpg.window(no_collapse=True,no_close=True,no_move=True,tag="Only",no_title_bar=True):
    for i in range(5):
        for j in range(5):
            color_picker = dpg.add_color_picker(pos=((cw+spacing)*i,(ch+spacing)*j),width=cw, height=ch,tag="color_picker_"+str(i)+"_"+str(j))
            f = Callable(print(i))
            button = dpg.add_button(pos=((cw+spacing)*i,(ch+spacing)*j + 200),label= "Remove", callback = f)
        chcp = i
    dpg.add_button(pos=(0,ch*(chcp-1)),label="Export Palette")
    dpg.configure_item("Only", width = 5000, height = 5000)


dpg.show_viewport()
dpg.start_dearpygui()
dpg.destroy_context()