import engine_api
class Test_1:    
    def step(self, hi: engine_api.Place):
        print("bools suck")
        # if hi.next_place == "":
        #     print("changed")
        #     hi.next_place = "spp"
        try:
            print("current_place is:", hi.Get_Place_Name)
            hi._cpp_place.instances[0].components[1].x_offset += 1
        except Exception as e:
            print("err:", e)
                 
    def create(self, hi):
        print("first bby")