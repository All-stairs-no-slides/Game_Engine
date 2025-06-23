#include <iostream>
#include <fstream>
#include <filesystem>
#include <nlohmann/json.hpp>
#include"Game_Object.h"
using json = nlohmann::json;
using namespace std;

int main()
{

	std::ifstream f(R"(C:\Users\amcd1\Desktop\projects\Game_Engine\tests\Objects\nnn\nnn.obj)");
	json plain_json = json::parse(f);
	cout << "Current path is: " << plain_json << endl;
	game_object::Game_Object obj = game_object::Game_Object::from_json(plain_json);

	// Accessing the deserialized components
	std::cout << "Game Object: " << obj.Name << "\n";
	for (const auto& comp : obj.components) {

		std::cout << "Component Type: " << comp->type << "\n";
	}


	return 0;
}