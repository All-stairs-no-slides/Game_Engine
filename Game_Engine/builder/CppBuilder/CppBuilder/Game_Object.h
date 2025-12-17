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

            // else if because its more simple than making an enum for a switch statement
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
            else if (type == "Script") {
                game_components::script_component comp = j.get<game_components::script_component>();
                comp.Initialisation();
                auto ret = std::make_shared<game_components::script_component>(comp);
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

        void Components_Loop() {
            for (const auto& comp : components) {
                game_components::transform_component current_transform;

                if (comp->type == "Transform") {
                    std::shared_ptr<game_components::transform_component> Transform = std::dynamic_pointer_cast<game_components::transform_component>(comp);

                    if (Transform) {
                        current_transform = *Transform.get();
                    }
                    continue;

                }
                if (comp->type == "Sprite_renderer") {
                    std::shared_ptr<game_components::sprite_renderer> spr_renderer = std::dynamic_pointer_cast<game_components::sprite_renderer>(comp);

                    if (spr_renderer) {

                        spr_renderer->DrawSelf(
                            glm::vec2((float)current_transform.x + (float)spr_renderer->x_offset, (float)current_transform.y + (float)spr_renderer->y_offset), // position
                            glm::vec2(current_transform.x_scale * spr_renderer->x_scale, current_transform.y_scale * spr_renderer->y_scale), // scale
                            current_transform.rotation + spr_renderer->rotation, // rotation
                            glm::vec3(1.0f, 1.0f, 1.0f)); // colour
                    }
                    continue;

                }
            }
        }

        NLOHMANN_DEFINE_TYPE_INTRUSIVE(Game_Object, Name, components);
    };

}
