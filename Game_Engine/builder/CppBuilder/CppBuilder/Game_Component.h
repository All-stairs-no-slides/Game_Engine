#pragma once
#include <string>
#include <nlohmann/json.hpp>

namespace game_components {

    class Game_Component {
    public:
        std::string type;  // For polymorphic deserialization
        Game_Component(std::string type) : type(type) {}
        Game_Component() = default;

        virtual ~Game_Component() = default;  // Virtual destructor for polymorphism

        virtual void from_json(const nlohmann::json& j) {
            j.at("type").get_to(type);  // Deserialize the 'type' field
        }

        // Make sure the base class has the correct serialization macro
        NLOHMANN_DEFINE_TYPE_INTRUSIVE(Game_Component, type)
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

        sprite_renderer() = default;
        sprite_renderer(std::string type, int x_offset, int y_offset, double x_scale, double y_scale, double rotation, std::string Sprite_dir, int depth) : Game_Component(type), x_offset(x_offset), y_offset(y_offset), x_scale(x_scale), y_scale(y_scale), rotation(rotation), Sprite_dir(Sprite_dir), depth(depth) 
        {
        }

        NLOHMANN_DEFINE_TYPE_INTRUSIVE(sprite_renderer, type, x_offset, y_offset, x_scale, y_scale, rotation, Sprite_dir, depth)
    };
}
