#include "Game_Component.h"
#include <iostream>
#include"Sprite_Asset.h"
#include<fstream>
#include"stb_image.h"
using namespace game_components;
using json = nlohmann::json;

void sprite_renderer::Initialisation() 
{
	// load sprites json
	std::ifstream f(this->Sprite_dir);
	json plain_json = json::parse(f);
	// deserialize sprite
	Sprite::Sprite spr;
	plain_json.at("name").get_to(spr.Name);
	plain_json.at("durations").get_to(spr.durations);
	plain_json.at("Images_location").get_to(spr.Image_location);
	this->sprite = spr;

}