import engine_api
class Test_1:    
    def step(self, hi: engine_api.Place, inputs : engine_api.User_Inputs):
        print("bools suck")
        # if hi.next_place == "":
        #     print("changed")
        #     hi.next_place = "spp"
        try:
            print("moving obj_name is:", hi.Get_Instance("olla").Get_Component(2).sound_alias)
            hi.Get_Instance("olla").Get_Component(2).Play()
            
            # engine_api.Sprite_Renderer()  
            hi.Get_Instance("noice").Transform.x = int(inputs.mouse_x)
            print("l_mouse:, ", inputs.L_mouse_pressed)

        except Exception as e:
            print("err:", e)
                 
    def create(self, hi: engine_api.Place, inputs):
        hi.Get_Instance("noice").Transform.x_scale = 2
        # hi.Get_Instance("noice").Transform.y_scale = 0.001
        engine_api.Sprite_Renderer(hi.Get_Instance("noice").Get_Sprite_Renderers()[0]).x_offset = 0
        hi.Get_Instance("olla").Get_Component(2).Set_pitch(3.0)
    
    def on_collide(self, this_obj : engine_api.Instance, Collision : engine_api.Contact, inputs):
        this_obj.Transform.x += 1
        # engine_api.Contact(Collision).obj_2.Transform.rotation += 1
        print("yoyoyo")