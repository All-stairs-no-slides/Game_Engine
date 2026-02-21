class Test_1:    
    def step(self, hi):
        print("bools suck")
        # if hi.next_place == "":
        #     print("changed")
        #     hi.next_place = "spp"
        try:
            print("next_place is:", hi.next_place)
            hi.instances[0].components[1].x_offset += 1
        except Exception as e:
            print("err:", e)
                 
    def create(self, hi):
        print("first bby")