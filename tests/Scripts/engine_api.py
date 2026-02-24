class Place:
    def __init__(self, cpp_place):
        self._cpp_place = cpp_place
        
    @property
    def Get_Instances(self):
        return self._cpp_place.instances
    
    @property
    def Get_Place_Name(self):
        return self._cpp_place.place_Name
    
    @property
    def Get_Next_Place(self):
        return self._cpp_place.next_place
    
    def Get_Instance(self, name: str):
        for i in self._cpp_place.instances:
            if i.name == name:
                return i
            
        