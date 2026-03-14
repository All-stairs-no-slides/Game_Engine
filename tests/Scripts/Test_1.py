import engine_api
class Test_1:    
    def step(self, hi: engine_api.Place):
        print("bools suck")
        # if hi.next_place == "":
        #     print("changed")
        #     hi.next_place = "spp"
        try:
            print("moving obj_name is:", hi.Get_Instances[1].name)
            engine_api.Sprite_Renderer(hi.Get_Instance("noice").Get_Sprite_Renderers()[0]).x_offset += 1
            # hi.Get_Instance("noice").Transform.rotation += 1

            

        except Exception as e:
            print("err:", e)
                 
    def create(self, hi):
        # hi.Get_Instance("noice").Transform.x_scale = 0.001
        # hi.Get_Instance("noice").Transform.y_scale = 0.001
        hi.Get_Instance("noice").Transform.rotation += 180
