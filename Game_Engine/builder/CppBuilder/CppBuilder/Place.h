#pragma once
#include<string>
#include<vector>
#include <nlohmann/json.hpp>
#include"Game_Object.h"
namespace Place {
	class Place {
	public:
		std::string Place_name;
		std::vector<game_object::Game_Object> Instances;
		
		Place(std::string Place_name, std::vector<game_object::Game_Object> Instances) : Place_name(Place_name), Instances(Instances) {}
		Place() = default;

		static Place from_json(const nlohmann::json& j) {
			Place place;
			j.at("Place_name").get_to(place.Place_name);

			for (const auto& inst : j.at("Instances")) {
				place.Instances.push_back(game_object::Game_Object::from_json(inst));
			}

			return place;
		}

		NLOHMANN_DEFINE_TYPE_INTRUSIVE(Place, Place_name, Instances);

	};
}