#pragma once
#include <string>
#include <iostream>
#include <nlohmann/json.hpp>
#include "Sprite_Asset.h"
#include "Shader_Utilities.h"
#include "Textures.h"

#include <pybind11/embed.h>

// glad must come before glfw
#include <glad/glad.h>
#include <glfw3.h>
// gl mathematics
#include <glm/glm.hpp>
#include <glm/gtc/matrix_transform.hpp>
#include <glm/gtc/type_ptr.hpp>

// texture loading
#include "stb_image.h"
// audio libraries
#include <miniaudio/miniaudio.h>


namespace py = pybind11;

namespace game_object
{  // forward declaration
    class Game_Object;
    
}

namespace Place
{
    
    class Place;
    struct User_Inputs;
}

namespace game_components {
    
    struct Contact; // forward declaration 👍
    class Game_Component {
    public:
        // settup for deserialisation
        // ------------------------------------------------------------------
        std::string type;  // For polymorphic deserialization
        Game_Component(std::string type) : type(type) 
        {
            Initialisation();
            //std::cout << "comp created and initialised through constructor" << std::endl;
        }
        Game_Component() = default;

        virtual ~Game_Component() = default;  // Virtual destructor for polymorphism

        virtual void from_json(const nlohmann::json& j) {
            j.at("type").get_to(type);  // Deserialize the 'type' field
        }

        // Make sure the base class has the correct serialization macro
        NLOHMANN_DEFINE_TYPE_INTRUSIVE(Game_Component, type);

        // Object functions
        // ------------------------------------------------------------------
        void Initialisation() {
            //std::cout << "initialising component" << std::endl;
        }

    };
    class audio_component : public Game_Component {
    public:
        std::string path;
        std::string sound_alias;
        

        audio_component() = default;
        audio_component(std::string type, std::string path, std::string sound_alias) 
        {
        }

        void Initialisation();
        void Play();
        void Set_pitch(float pitch);
        void Set_start_time_mill(ma_uint64 time);
        ma_uint64 Get_time_mil();
        void Pause();


        NLOHMANN_DEFINE_TYPE_INTRUSIVE(audio_component, path, sound_alias)

        std::shared_ptr<ma_sound> sound;
        std::shared_ptr<ma_engine> s_engine;

    };

    class script_component : public Game_Component {
    public:
        std::string path;
        std::string scope; // can be local, room, or global
        bool create_iter = true;

        script_component() = default;
        script_component(std::string type, std::string path, std::string scope_exposure) : Game_Component(type), path(path), scope(scope)
        {
        }

        void Initialisation();

        NLOHMANN_DEFINE_TYPE_INTRUSIVE(script_component, type, path, scope)
    
        py::module_ script_module;

        void Event_Call(const char* event_name, game_object::Game_Object* parsed_item, py::module_ engine_api, Place::User_Inputs* User_Inputs);
        void Event_Call(const char* event_name, Place::Place* parsed_item, py::module_ engine_api, Place::User_Inputs* User_Inputs);
        void Event_Call(const char* event_name, game_object::Game_Object* this_obj, game_components::Contact* collsion, py::module_ engine_api, Place::User_Inputs* User_Inputs);
    };

    class transform_component : public Game_Component {
    public:
        int x, y, z;
        double x_scale, y_scale, rotation;

        transform_component() = default;
        transform_component(std::string type, int x, int y, int z, double x_scale, double y_scale, double rotation) : Game_Component(type), x(x), y(y), z(z), x_scale(x_scale), y_scale(y_scale), rotation(rotation) 
        {
        }

        NLOHMANN_DEFINE_TYPE_INTRUSIVE(transform_component, type, x, y, z, x_scale, y_scale, rotation)
    };

    
    class Collider : public Game_Component {
    public:
        std::string Collider_type;
        std::string Collider_alias;
        std::vector<double> Proportions;


        Collider() = default;
        Collider(std::string type, std::string Collider_type, std::string Collider_alias, std::vector<double> Proportions) : Game_Component(type), Collider_type(Collider_type), Collider_alias(Collider_alias), Proportions(Proportions)
        {
        }

        void from_json(const nlohmann::json& j) {
            j.at("type").get_to(type);
            j.at("Collider_type").get_to(Collider_type);
            j.at("Collider_alias").get_to(Collider_alias);
            j.at("Proportions").get_to(Proportions);
        }

        NLOHMANN_DEFINE_TYPE_INTRUSIVE(Collider, type, Collider_type, Collider_alias, Proportions);
    };

    class sprite_renderer : public Game_Component {
    public:
        int x_offset, y_offset;
        double x_scale, y_scale, rotation;
        std::string Sprite_dir;
        int depth;
        Shader_utils::Shader shader = Shader_utils::Shader(R"(C:\Users\amcd1\Desktop\projects\Game_Engine\tests\Shaders\Basic_Shader\Basic.vsh)", R"(C:\Users\amcd1\Desktop\projects\Game_Engine\tests\Shaders\Basic_Shader\Basic.fsh)");

        sprite_renderer() = default;
        sprite_renderer(std::string type, int x_offset, int y_offset, double x_scale, double y_scale, double rotation, std::string Sprite_dir, int depth) : Game_Component(type), x_offset(x_offset), y_offset(y_offset), x_scale(x_scale), y_scale(y_scale), rotation(rotation), Sprite_dir(Sprite_dir), depth(depth) 
        {
        }

        NLOHMANN_DEFINE_TYPE_INTRUSIVE(sprite_renderer, type, x_offset, y_offset, x_scale, y_scale, rotation, Sprite_dir, depth);


        

        void Initialisation();

        void DrawSelf(glm::vec2 position,
            glm::vec2 size = glm::vec2(10.0f, 10.0f), float rotate = 0.0f,
            glm::vec3 color = glm::vec3(1.0f));

    private:
        Sprite::Sprite sprite;
        unsigned int VAO;

    };

    struct Contact {
        // located here because in any other file it is slightly more difficult 2 make a forward declaration 4 
            // idk just dont move it future me
        game_components::Collider* col_1;
        game_object::Game_Object* obj_1;
        game_components::Collider* col_2;
        game_object::Game_Object* obj_2;

        bool operator<(const Contact& other) const {
            if (obj_1 != other.obj_1) return obj_1 < other.obj_1;
            return obj_2 < other.obj_2;
        }
    };
}
