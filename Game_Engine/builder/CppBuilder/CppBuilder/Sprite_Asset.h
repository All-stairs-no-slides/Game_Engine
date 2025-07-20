#pragma once
#include <string>
#include <vector>
#include "Textures.h"
namespace Sprite {
	class Sprite {
	public:
		std::string Name;
		std::vector<float> durations;
		std::vector<std::string> Image_location;
		Sprite(std::string Name, std::vector<std::string> Image_location, std::vector<float> durations) : Name(Name), durations(durations), Image_location(Image_location) {};
		Sprite() = default;
		NLOHMANN_DEFINE_TYPE_INTRUSIVE(Sprite, Name, Image_location, durations);

		void Initialise();
	private:
		std::vector<Textures::Texture2D> textures;

	};
}