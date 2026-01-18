class Test_1:    
    def step(self, hi):
        print("bools suck")
        
        try:
            hi.instances[0].components[1].x_offset += 1
        except Exception as e:
            print("err:", e)
                 