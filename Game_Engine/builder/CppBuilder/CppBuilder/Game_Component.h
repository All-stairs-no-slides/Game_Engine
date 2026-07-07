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

        virtual std::shared_ptr<Game_Component> clone() const { return nullptr; }

    };
    class audio_component : public Game_Component {
    public:
        std::string path;
        std::string sound_alias;
        

        audio_component() = default;
        audio_component(std::string type, std::string path, std::string sound_alias) 
        {
        }

        void Initialisation(std::string Asset_path);
        void Play();
        void Set_pitch(float pitch);
        void Set_time_sec(float time);
        ma_uint64 Get_time_mil();
        void Pause();

        std::shared_ptr<Game_Component> clone() const override {
            // Construct a fresh audio_component with the same data fields,
            // then re-run Initialisation so it gets its own ma_engine and ma_sound.
            audio_component copy;
            copy.type         = type;
            copy.path         = path;
            copy.sound_alias  = sound_alias;
            copy.stored_asset_path = stored_asset_path;
            if (!stored_asset_path.empty()) {
                copy.Initialisation(stored_asset_path);
            }
            return std::make_shared<audio_component>(std::move(copy));
        }

        NLOHMANN_DEFINE_TYPE_INTRUSIVE(audio_component, path, sound_alias)

        std::shared_ptr<ma_sound> sound;
        std::shared_ptr<ma_engine> s_engine;

        // Stored so clone() can re-initialise with the correct path
        std::string stored_asset_path;
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

        std::shared_ptr<Game_Component> clone() const override {
            // Each instance needs its own script_component with create_iter reset
            // so the "create" event fires for every new instance.
            script_component copy;
            copy.type         = type;
            copy.path         = path;
            copy.scope        = scope;
            copy.create_iter  = true;   // reset so "create" fires on the new instance
            copy.Initialisation();
            return std::make_shared<script_component>(std::move(copy));
        }

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

        std::shared_ptr<Game_Component> clone() const override {
            return std::make_shared<transform_component>(*this);
        }
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

        std::shared_ptr<Game_Component> clone() const override {
            return std::make_shared<Collider>(*this);
        }
    };

    class sprite_renderer : public Game_Component {
    public:
        int x_offset, y_offset;
        double x_scale, y_scale, rotation;
        std::string Sprite_dir;
        int depth;
        //Shader_utils::Shader shader = Shader_utils::Shader((std::filesystem::current_path().string() + "\\Shaders\\Basic_Shader\\Basic.vsh").c_str(), (std::filesystem::current_path().string() + "\\Shaders\\Basic_Shader\\Basic.fsh").c_str());

        Shader_utils::Shader* shader;

        sprite_renderer() = default;
        sprite_renderer(std::string type, int x_offset, int y_offset, double x_scale, double y_scale, double rotation, std::string Sprite_dir, int depth) : Game_Component(type), x_offset(x_offset), y_offset(y_offset), x_scale(x_scale), y_scale(y_scale), rotation(rotation), Sprite_dir(Sprite_dir), depth(depth) 
        {
        }

        NLOHMANN_DEFINE_TYPE_INTRUSIVE(sprite_renderer, type, x_offset, y_offset, x_scale, y_scale, rotation, Sprite_dir, depth);

        std::shared_ptr<Game_Component> clone() const override {
            // Re-run Initialisation so the clone gets its own shader and VAO.
            sprite_renderer copy;
            copy.type       = type;
            copy.x_offset   = x_offset;
            copy.y_offset   = y_offset;
            copy.x_scale    = x_scale;
            copy.y_scale    = y_scale;
            copy.rotation   = rotation;
            copy.Sprite_dir = Sprite_dir;
            copy.depth      = depth;
            copy.stored_proj_path = stored_proj_path;
            if (!stored_proj_path.empty()) {
                copy.Initialisation(stored_proj_path);
            }
            return std::make_shared<sprite_renderer>(std::move(copy));
        }

        // Stored so clone() can re-initialise with the correct project path
        std::string stored_proj_path;

        void Initialisation(std::string Proj_path);

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
