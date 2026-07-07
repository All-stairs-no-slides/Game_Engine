#pragma once
#include <string>
#include <vector>
#include <nlohmann/json.hpp>
#include "Game_Component.h"
// pybind
#include <Python.h>
#include <pybind11/embed.h>
#include <pybind11/stl.h>
#include <pybind11/stl_bind.h>

namespace py = pybind11;
using namespace py::literals;

namespace Place
{
    class Place;
}

namespace game_object {
    
    class Game_Object {
    public:
        std::string Name;
        std::vector<std::shared_ptr<game_components::Game_Component>> components;
        std::set<game_components::Contact> Collisions;
        py::dict Locals;


        Game_Object(std::string Name, std::vector<std::shared_ptr<game_components::Game_Component>> components) : Name(Name), components(components) 
        {
            std::cout << "olla" << std::endl;
        }

        Game_Object() = default;

        // Deserialize a single component based on its type field
        static std::shared_ptr<game_components::Game_Component> deserialize_component(const nlohmann::json& j, std::string path) {
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
                comp.Initialisation(path);
                auto ret = std::make_shared<game_components::sprite_renderer>(comp);
                return ret;
            }
            else if (type == "Script") {
                game_components::script_component comp = j.get<game_components::script_component>();
                comp.Initialisation();
                auto ret = std::make_shared<game_components::script_component>(comp);
                return ret;
            }
            else if (type == "Collider") {
                game_components::Collider comp;
                comp.from_json(j);
                comp.Initialisation();
                auto ret = std::make_shared<game_components::Collider>(comp);
                return ret;
            }
            else if (type == "Audio") {
                game_components::audio_component comp = j.get<game_components::audio_component>();
                comp.Initialisation(path + "\\Assets\\");
                auto ret = std::make_shared<game_components::audio_component>(comp);
                return ret;
            }

            throw std::runtime_error("Unknown component type: " + type);  // Error if type is unknown
        }

        // Deserialize the entire Game_Object
        static Game_Object from_json(const nlohmann::json& j, const std::string path) {
            Game_Object obj;

            // Deserialize the Name field
            j.at("Name").get_to(obj.Name);

            // Deserialize the components array
            for (const auto& comp : j.at("components")) {
                std::shared_ptr<game_components::Game_Component> temp_c = deserialize_component(comp, path);
                obj.components.push_back(temp_c);
            }

            return obj;
        }

        void Components_Loop(Place::Place *global_context, py::module_ engine_api, Place::User_Inputs* User_Inputs) {
            // a backup in the case of a freak accident when there is a missing transform
            game_components::transform_component current_transform = game_components::transform_component("Transform", 0, 0, 0, 1, 1, 0);

            for (const auto& comp : components) {
                
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
                if (comp->type == "Script") {
                    std::shared_ptr<game_components::script_component> script_comp = std::dynamic_pointer_cast<game_components::script_component>(comp);
                    if (script_comp) {
                        if (script_comp->create_iter == true) {
                            script_comp->create_iter = false;
                            if (script_comp->scope == "Local") {
                                script_comp->Event_Call("create", this, engine_api, User_Inputs);
                            }
                            else if (script_comp->scope == "Global") {
                                script_comp->Event_Call("create", global_context, engine_api, User_Inputs);
                            }
                        }

                        if (script_comp->scope == "Local") {
                            script_comp->Event_Call("step", this, engine_api, User_Inputs);
                            if (Collisions.size() != 0) {
                                for(game_components::Contact c_obj : Collisions) {
                                    script_comp->Event_Call("on_collide", this,& c_obj, engine_api, User_Inputs);
                                };
                            }
                        }
                        else if (script_comp->scope == "Global") {
                            script_comp->Event_Call("step", global_context, engine_api, User_Inputs);
                            if (Collisions.size() != 0) {
                                for (game_components::Contact c_obj : Collisions) {
                                    script_comp->Event_Call("on_collide", this, &c_obj, engine_api, User_Inputs);
                                };
                            }
                        }
                    }
                    continue;
                }

                if (comp->type == "Collider") {
                    continue;
                }
                if (comp->type == "Audio") {
                    continue;
                }
            }
            // set all collisions for this object back to 0 as they have been handled in the scripting components
            Collisions.clear();
            return;
        }

            


        /// Returns a deep copy of this Game_Object where every component is an
        /// independently owned object (each shared_ptr points to a fresh clone).
        Game_Object deep_copy() const {
            Game_Object copy;
            copy.Name = Name;
            for (const auto& comp : components) {
                copy.components.push_back(comp->clone());
            }
            // Collisions are per-frame and intentionally NOT copied
            return copy;
        }

        NLOHMANN_DEFINE_TYPE_INTRUSIVE(Game_Object, Name, components);
    };

}
