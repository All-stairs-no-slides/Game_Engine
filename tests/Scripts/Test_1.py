class test:    
    def step(self, hi):
        print("bools suck")
        hi.__class__ = Game_Object 
        hi.components[1].x_offset += 1

        
        hi.Name = "yello"