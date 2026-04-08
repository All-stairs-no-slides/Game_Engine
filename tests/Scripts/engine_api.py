class Place:
    def __init__(self, cpp_place):
        self._cpp_place = cpp_place
        
    @property
    def Get_Instances(self):
        return self._cpp_place.instances
    
    @property
    def Get_Name(self):
        return self._cpp_place.place_Name
    
    @property
    def Get_Next_Place(self):
        return self._cpp_place.next_place
    
    def Get_Instance(self, name: str):
        for i in self._cpp_place.instances:
            if i.name == name:
                return Instance(i)
            
        
class Instance:
    def __init__(self, cpp_instance):
        self._cpp_instance = cpp_instance

    @property
    def Name(self):
        return self._cpp_instance.name
    
    @property
    def Components(self):
        return self._cpp_instance.components
    
    @property
    def Transform(self):
        for i in self._cpp_instance.components:
            if i.type == "Transform":
                return Transform(i)
    
    def Get_Component(self, index: int):
        return self._cpp_instance.components[index]
    
    def Get_Sprite_Renderers(self):
        ret = []
        for i in self._cpp_instance.components:
            if i.type == "Sprite_renderer":
                ret.append(Sprite_Renderer(i))
                

        return ret


    

class Component:
    def __init__(self, cpp_component):
        self._cpp_component = cpp_component

    @property
    def Type(self):
        return self._cpp_component.type

class Collider(Component):
    def __init__(self, cpp_component):
        self._cpp_component = cpp_component

    @property
    def Collider_type(self):
        return self._cpp_component.Collider_type
    
    @property
    def Collider_type(self):
        return self._cpp_component.Collider_alias
    
    @property
    def Collider_type(self):
        return self._cpp_component.Proportions


class Transform(Component):
    def __init__(self, cpp_component):
        self._cpp_component = cpp_component

    @property
    def x(self):
        return self._cpp_component.x
    
    @x.setter
    def x(self, value):
        self._cpp_component.x = value

    @property
    def y(self):
        return self._cpp_component.y
    
    @y.setter
    def y(self, value):
        self._cpp_component.y = value

    @property
    def z(self):
        return self._cpp_component.z
    
    @z.setter
    def z(self, value):
        self._cpp_component.z = value

    @property
    def rotation(self):
        return self._cpp_component.rotation
    
    @rotation.setter
    def rotation(self, value):
        self._cpp_component.rotation = value
    
    @property
    def x_scale(self):
        return self._cpp_component.x_scale
    
    @x_scale.setter
    def x_scale(self, value):
        self._cpp_component.x_scale = value

    @property
    def y_scale(self):
        return self._cpp_component.y_scale
    
    @y_scale.setter
    def y_scale(self, value):
        self._cpp_component.y_scale = value


class Sprite_Renderer(Component):
    def __init__(self, cpp_component):
        self._cpp_component = cpp_component

    @property
    def x_offset(self):
        return self._cpp_component.x_offset
    
    @x_offset.setter
    def x_offset(self, value):
        self._cpp_component.x_offset = value

    @property
    def y_offset(self):
        return self._cpp_component.y_offset
    
    @y_offset.setter
    def y_offset(self, value):
        self._cpp_component.y_offset = value

    @property
    def depth(self):
        return self._cpp_component.depth
    
    @depth.setter
    def depth(self, value):
        self._cpp_component.depth = value

    @property
    def rotation(self):
        return self._cpp_component.rotation
    
    @rotation.setter
    def rotation(self, value):
        self._cpp_component.rotation = value
    
    @property
    def x_scale(self):
        return self._cpp_component.x_scale
    
    @x_scale.setter
    def x_scale(self, value):
        self._cpp_component.x_scale = value

    @property
    def y_scale(self):
        return self._cpp_component.y_scale
    
    @y_scale.setter
    def y_scale(self, value):
        self._cpp_component.y_scale = value

    @property
    def shader(self):
        return self._cpp_component.shader
    
    @shader.setter
    def shader(self, value):
        self._cpp_component.shader = value

    @property
    def sprite_dir(self):
        return self._cpp_component.sprite_dir
    
    @sprite_dir.setter
    def sprite_dir(self, value):
        self._cpp_component.sprite_dir = value
    
class User_Inputs:
    def __init__(self, cpp_class):
        self._cpp_class = cpp_class

    @property
    def L_mouse_pressed(self):
        return self._cpp_class.L_mouse
    
    @property
    def R_mouse_pressed(self):
        return self._cpp_class.R_mouse
    
    @property
    def mouse_x(self):
        return self._cpp_class.mousex
    
    @property
    def mouse_y(self):
        return self._cpp_class.mousey
    
    @property
    def pressed_keys(self):
        return self._cpp_class.Keys_pressed
    
    

    

class Contact:
    def __init__(self, cpp_contact):
        self._cpp_component = cpp_contact

    @property
    def obj_1(self):
        return Instance(self.cpp_contact.obj_1)

    @property
    def obj_2(self):
        return Instance(self.cpp_contact.obj_2)
    
    @property
    def col_1(self):
        return Collider(self.cpp_contact.col_1)
    
    @property
    def col_2(self):
        return Collider(self.cpp_contact.col_2)
    
    @obj_1.setter
    def obj_1(self, value):
        self.cpp_contact.obj_1 = value

    @obj_2.setter
    def obj_2(self, value):
        self.cpp_contact.obj_2 = value

    @col_1.setter
    def col_1(self, value):
        self.cpp_contact.col_1 = value

    @col_2.setter
    def col_2(self, value):
        self.cpp_contact.col_2 = value