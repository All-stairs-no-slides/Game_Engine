#include "Sprite_Asset.h"
#include<string>
// texture loading
#include "stb_image.h"
void Sprite::Sprite::Initialise() {
	for (std::string path : this->Image_location) {
		// width height and colour channels
		int width, height, nrChannels;
		unsigned char* data = stbi_load(path.c_str(), &width, &height, &nrChannels, 0);
		Textures::Texture2D texture = Textures::Texture2D();
		texture.Generate(width, height, data);
		this->textures.push_back(texture);
	}
	this->current_texture = 0;
}

Textures::Texture2D Sprite::Sprite::Get_Current_texture() {
	return this->textures[this->current_texture];
}

void Sprite::Sprite::Iterate_texture() {
	if (passed_frames > durations[current_texture]) {
		if (current_texture < textures.size()) {
			current_texture += 1;
		}
		passed_frames = 0;
	}
}