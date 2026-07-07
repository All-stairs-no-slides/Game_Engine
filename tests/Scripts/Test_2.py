import engine_api

class Test_2:    
    def create(self, hoi: engine_api.Instance, inputs):
        print("createeeeeeeeeeeeeeeeeeeeeeeecreateeeeeeeeeeeeeeeeeeeeeeeecreateeeeeeeeeeeeeeeeeeeeeeeecreateeeeeeeeeeeeeeeeeeeeeeeecreateeeeeeeeeeeeeeeeeeeeeeeecreateeeeeeeeeeeeeeeeeeeeeeeecreateeeeeeeeeeeeeeeeeeeeeeeecreateeeeeeeeeeeeeeeeeeeeeeeecreateeeeeeeeeeeeeeeeeeeeeeeecreateeeeeeeeeeeeeeeeeeeeeeeecreateeeeeeeeeeeeeeeeeeeeeeeecreateeeeeeeeeeeeeeeeeeeeeeeecreateeeeeeeeeeeeeeeeeeeeeeeecreateeeeeeeeeeeeeeeeeeeeeeeecreateeeeeeeeeeeeeeeeeeeeeeeecreateeeeeeeeeeeeeeeeeeeeeeeecreateeeeeeeeeeeeeeeeeeeeeeeecreateeeeeeeeeeeeeeeeeeeeeeeecreateeeeeeeeeeeeeeeeeeeeeeeecreateeeeeeeeeeeeeeeeeeeeeeeecreateeeeeeeeeeeeeeeeeeeeeeeecreateeeeeeeeeeeeeeeeeeeeeeeecreateeeeeeeeeeeeeeeeeeeeeeeecreateeeeeeeeeeeeeeeeeeeeeeeecreateeeeeeeeeeeeeeeeeeeeeeeecreateeeeeeeeeeeeeeeeeeeeeeeecreateeeeeeeeeeeeeeeeeeeeeeeecreateeeeeeeeeeeeeeeeeeeeeeeecreateeeeeeeeeeeeeeeeeeeeeeee")
        # hoi.Name = "nloc"
    def step(self, hoi: engine_api.Instance, inputs : engine_api.User_Inputs):
        # print("bools suck")
        try:
            hoi.Transform.x = int(inputs.mouse_x)
            # print("l_mouse: ", inputs.L_mouse_pressed)

            

            # print("type is:", hi.Get_Instance("olla").Locals["bobo"])

        except Exception as e:
            print("err:", e)