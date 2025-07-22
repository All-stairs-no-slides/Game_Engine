#pragma once
#include <string>
#include <vector>
#include "Textures.h"
#include <nlohmann/json.hpp>
namespace Sprite {
	class Sprite {

	public:
		std::string Name;
		std::vector<float> durations;
		std::vector<std::string> Image_location;
		Sprite() = default;
		Sprite(std::string Name, std::vector<std::string> Image_location, std::vector<float> durations) : Name(Name), durations(durations), Image_location(Image_location) 
		{
		}
		NLOHMANN_DEFINE_TYPE_INTRUSIVE(Sprite, Name, Image_location, durations);

		void Initialise();
		void Iterate_texture();
		Textures::Texture2D Get_Current_texture();
		
	private:
		std::vector<Textures::Texture2D> textures;
		int passed_frames = 0;
		int current_texture;
	};
}