#pragma once
#include <string>
#include <vector>
#include <nlohmann/json.hpp>
#include "Game_Component.h"

namespace game_object {

    class Game_Object {
    public:
        std::string Name;
        std::vector<std::shared_ptr<game_components::Game_Component>> components;

        Game_Object(std::string Name, std::vector<std::shared_ptr<game_components::Game_Component>> components) : Name(Name), components(components) 
        {
            std::cout << "olla" << std::endl;
        }

        Game_Object() = default;

        // Deserialize a single component based on its type field
        static std::shared_ptr<game_components::Game_Component> deserialize_component(const nlohmann::json& j) {
            std::string type;

            if (!(j.contains("type"))) {
                throw std::runtime_error("Unknown component type: " + type);  // Error if type is unknown

            }
            j.at("type").get_to(type);  // Extract the type field

            if (type == "Transform") {
                game_components::transform_component comp = j.get<game_components::transform_component>();
                comp.Initialisation();
                auto ret = std::make_shared<game_components::transform_component>(comp);
                
                return ret;
            }
            else if (type == "Sprite_renderer") {
                game_components::sprite_renderer comp = j.get<game_components::sprite_renderer>();
                comp.Initialisation();
                auto ret = std::make_shared<game_components::sprite_renderer>(comp);
                return ret;
            }

            throw std::runtime_error("Unknown component type: " + type);  // Error if type is unknown
        }

        // Deserialize the entire Game_Object
        static Game_Object from_json(const nlohmann::json& j) {
            Game_Object obj;

            // Deserialize the Name field
            j.at("Name").get_to(obj.Name);

            // Deserialize the components array
            for (const auto& comp : j.at("components")) {
                obj.components.push_back(deserialize_component(comp));
            }

            return obj;
        }

        NLOHMANN_DEFINE_TYPE_INTRUSIVE(Game_Object, Name, components);
    };

}
