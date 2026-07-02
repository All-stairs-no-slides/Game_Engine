import engine_api
class Test_1:    
    def step(self, hi: engine_api.Place, inputs : engine_api.User_Inputs):
        print("bools suck")
        try:
            hi.Get_Instance("noice").Transform.x = int(inputs.mouse_x)
            # print("l_mouse: ", inputs.L_mouse_pressed)
            print("type is:", hi.Globals["yoyoyoo"])
            print("type is:", hi.Globals["noi"])

            # print("type is:", hi.Get_Instance("olla").Locals["bobo"])



        except Exception as e:
            print("err:", e)
                 
    def create(self, hi: engine_api.Place, inputs):
        hi.Globals["yoyoyoo"] = 6
        hi.Globals["noi"] = "joi"
        hi.Globals["yoyoyoo"] += 1

        # hi.Get_Instance("olla").Locals["bobo"] = "yo"

    
    def on_collide(self, this_obj : engine_api.Instance, Collision : engine_api.Contact, inputs):
        this_obj.Transform.x += 1
        # engine_api.Contact(Collision).obj_2.Transform.rotation += 1
        print("yoyoyo")