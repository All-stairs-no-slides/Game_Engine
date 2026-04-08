#pragma once
#include<string>
#include<vector>
#include <nlohmann/json.hpp>
#include"Game_Object.h"
namespace Place {
	struct User_Inputs {
		double mousex;
		double mousey;
		bool L_mouse_pressed;
		bool R_mouse_pressed;
		std::vector<int> pressed_keys;

	};

	
	class Place {
	public:
		std::string Place_name;
		std::string Next_place_name;
		
		std::set<game_components::Contact> Global_Collisions;
		std::vector<game_object::Game_Object> Instances;
		

		Place(std::string Place_name, std::vector<game_object::Game_Object> Instances) : Place_name(Place_name), Instances(Instances) {}
		Place() = default;

		static Place from_json(const nlohmann::json& j) {
			Place place;
			j.at("Place_name").get_to(place.Place_name);
			place.Next_place_name = "";
			for (const auto& inst : j.at("Instances")) {
				place.Instances.push_back(game_object::Game_Object::from_json(inst));
			}

			return place;
		}


		void Collider_Loop() {
			// the collider loop will return all collisions as a vector of pairs of game object pointers
			std::map<std::pair<int, int>, std::vector<std::pair<std::pair<std::shared_ptr<game_components::transform_component>, std::shared_ptr<game_components::Collider>> ,game_object::Game_Object*>>> collider_grid;
			//int kount = 0;
			Global_Collisions.clear();
			for(game_object::Game_Object& obj :  Instances)
			{
				/*std::cout << "Iteration: " << i++
					<< " size: " << Instances.size()
					<< " addr: " << &obj << std::endl;*/

				std::shared_ptr<game_components::transform_component> current_transform;

				for (const auto& comp : obj.components) {
					// transforms should affect the collider to so  we check for the current one in play for this collider
					if (comp->type == "Transform") {
						current_transform = std::dynamic_pointer_cast<game_components::transform_component>(comp);
						continue;

					}
					if (comp->type == "Collider") {
						std::shared_ptr<game_components::Collider> collider_comp = std::dynamic_pointer_cast<game_components::Collider>(comp);
						if (collider_comp->Collider_type == "Rect") {
							// proportions for rect are [x_offsey, y_offset, width, height]
							int grid_x = floor((current_transform->x + collider_comp->Proportions[0]) / 50);
							int grid_y = floor((current_transform->y + collider_comp->Proportions[1]) / 50);
							int grid_width = ceil((current_transform->x_scale * collider_comp->Proportions[2]) / 50);
							int grid_height = ceil((current_transform->y_scale * collider_comp->Proportions[3]) / 50);
							
							for (int i = grid_x + 0; i < grid_x + grid_width; i++) {
								for (int j = grid_y + 0; j < grid_y + grid_height; j++) {
									auto found = collider_grid.find(std::pair<int, int>(i, j));
									if (found == collider_grid.end()) {
										// creating a new point in the map
										
										collider_grid.insert({ 
											std::pair<int, int>{i, j}, 
											std::vector <std::pair <std::pair<std::shared_ptr<game_components::transform_component>, std::shared_ptr<game_components::Collider>>,game_object::Game_Object*> > 
												{ std::pair<std::pair<std::shared_ptr<game_components::transform_component>, std::shared_ptr<game_components::Collider>> ,game_object::Game_Object*> 
													{ std::pair<std::shared_ptr<game_components::transform_component>, std::shared_ptr<game_components::Collider>> {current_transform, collider_comp },& obj } } });
										continue;
									}
									else {
										for (std::pair<std::pair<std::shared_ptr<game_components::transform_component>, std::shared_ptr<game_components::Collider>>, game_object::Game_Object*> col_pair : collider_grid[std::pair<int, int>{i, j}]) {
											int x_diff = (col_pair.first.first->x + col_pair.first.second->Proportions[0]) - (current_transform->x + collider_comp->Proportions[0]);
											int y_diff = (col_pair.first.first->y + col_pair.first.second->Proportions[1]) - (current_transform->y + collider_comp->Proportions[1]);
											// uses the diff to decide which collider to measure the widths and heights on (measured based on the one that hase a smaller offset value on that axis since colliders can only have positive widths and heights(think about changing that later))
											int width = (x_diff < 1) ? collider_comp->Proportions[2] : col_pair.first.second->Proportions[2];
											int height = (y_diff < 1) ? collider_comp->Proportions[3] : col_pair.first.second->Proportions[3];
											// check if there is a collision
											if (abs(x_diff) <= width and abs(y_diff) <= height) {
												

												
												
												// add collision to global context
												game_components::Contact contact;
												contact.col_1 = collider_comp.get();
												contact.obj_1 = &obj;

												contact.col_2 = col_pair.first.second.get();
												contact.obj_2 = col_pair.second;

												Global_Collisions.insert(contact);

												// add collision to local contexts
												col_pair.second->Collisions.insert(contact);
												obj.Collisions.insert(contact);

												//kount += 1;
												
											}
										}
										
										collider_grid[std::pair<int, int>{i, j}].push_back(std::pair<std::pair<std::shared_ptr<game_components::transform_component>, std::shared_ptr<game_components::Collider>>, game_object::Game_Object*> { std::pair<std::shared_ptr<game_components::transform_component>, std::shared_ptr<game_components::Collider>> {current_transform, collider_comp }, & obj });
										continue;
									}
								}
							}
						}
					}
				}
			}
			//std::cout << Global_Collisions.size() << std::endl;
		}

		NLOHMANN_DEFINE_TYPE_INTRUSIVE(Place, Place_name, Instances);

	};
}