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
#include "Shader_Utilities.h"

// texture loading
#include "stb_image.h"

namespace py = pybind11;




namespace game_components {

    class Game_Component {
    public:
        // settup for deserialisation
        // ------------------------------------------------------------------
        std::string type;  // For polymorphic deserialization
        Game_Component(std::string type) : type(type) 
        {
            std::cout << "yoyoyo" << std::endl;
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
            std::cout << "initialising component" << std::endl;
        }

    };

    class script_component : public Game_Component {
    public:
        std::string script_name;
        std::string scope_exposure; // can be local, room, or global

        script_component() = default;
        script_component(std::string type, std::string script_name, std::string scope_exposure) : Game_Component(type), script_name(script_name), scope_exposure(scope_exposure)
        {
        }

        NLOHMANN_DEFINE_TYPE_INTRUSIVE(script_component, type, script_name, scope_exposure)
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
}
