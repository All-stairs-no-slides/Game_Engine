import engine_api
class Test_1:    
    def step(self, this_obj: engine_api.Instance, place: engine_api.Place, inputs : engine_api.User_Inputs):
        # print("bools suck")
        try:
            this_obj.Transform.x = int(inputs.mouse_x)
            # print("l_mouse: ", inputs.L_mouse_pressed)
            # print("type is:", place.Globals["yoyoyoo"])
            # print("type is:", place.Globals["noi"])

            if(inputs.L_mouse_pressed):
                place.Instantiate("hhh")
                print("de")
                this_obj.destroy()
                print("stroyed")
                # place.Get_Instance("olla").Get_Audio_Components()[0].Play()

            if(inputs.R_mouse_pressed):
                place.Instantiate("nnn")
                
            place.Name = "nonononono"
            # if(place.Get_Instance("olla") != None):
            #     place.Get_Instance("olla").Name = "nonononono"
            # print("type is:", place.Get_Instance("olla").Name)



        except Exception as e:
            print("err:", e)

    
                 
    def create(self, this_obj: engine_api.Instance, place: engine_api.Place, inputs):
        place.Globals["yoyoyoo"] = 6
        place.Globals["noi"] = "joi"
        place.Globals["yoyoyoo"] += 1

        # place.Get_Instance("olla").Locals["bobo"] = "yo"

    
    def on_collide(self, this_obj : engine_api.Instance, Collision : engine_api.Contact, inputs):
        this_obj.Transform.x += 1
        if(inputs.L_mouse_pressed):
            print("EEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEE")
            audio_components = this_obj.Get_Audio_Components()
            
            audio_components[0].Play()
        # engine_api.Contact(Collision).obj_2.Transform.rotation += 1
        # 