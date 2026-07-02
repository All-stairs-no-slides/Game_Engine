#include "Sprite_Asset.h"
#include<string>
#include <iostream>
// texture loading
#include "stb_image.h"
void Sprite::Sprite::Initialise(std::string proj_path) {
	for (std::string path : this->Image_location) {
		// width height and colour channels
		int width, height, nrChannels;
		unsigned char* data = stbi_load((proj_path + "\\Assets\\" + path).c_str(), &width, &height, &nrChannels, 0);
		Textures::Texture2D texture = Textures::Texture2D();
		texture.Generate(width, height, data);
		this->textures.push_back(texture);
	}
	this->current_texture = 0;
}

Textures::Texture2D Sprite::Sprite::Get_Current_texture() {
	try {
		return this->textures[this->current_texture];
	}
	catch (int err) {
		std::cout << "cant find the image for the sprite: " << err << std::endl;
	}
}

void Sprite::Sprite::Iterate_texture() {
	if (passed_frames > durations[current_texture]) {
		if (current_texture < textures.size()) {
			current_texture += 1;
		}
		passed_frames = 0;
	}
}