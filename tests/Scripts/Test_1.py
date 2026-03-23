import engine_api
class Test_1:    
    def step(self, hi: engine_api.Place):
        print("bools suck")
        # if hi.next_place == "":
        #     print("changed")
        #     hi.next_place = "spp"
        try:
            print("moving obj_name is:", hi.Get_Instance("olla").Get_Component(3).Collider_alias)
            # engine_api.Sprite_Renderer()  
            hi.Get_Instance("noice").Transform.x += 1

        except Exception as e:
            print("err:", e)
                 
    def create(self, hi):
        # hi.Get_Instance("noice").Transform.x_scale = 0.001
        # hi.Get_Instance("noice").Transform.y_scale = 0.001
        hi.Get_Instance("noice").Transform.rotation += 1
    
    def on_collide(self, this_obj : engine_api.Instance, Collision : engine_api.Contact):
        this_obj.Transform.x += 1
        # engine_api.Contact(Collision).obj_2.Transform.rotation += 1
        print("yoyoyo")