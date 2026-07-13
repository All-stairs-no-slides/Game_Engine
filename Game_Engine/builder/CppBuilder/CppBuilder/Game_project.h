#include<string>
#include <nlohmann/json.hpp>

namespace Game_project {
	class Game_project {
	public:
		std::string Name;
		std::string Current_place;
		int Build_num;
		int Window_width;
		int Window_height;
		int Viewport_width;
		int Viewport_height;

		Game_project(std::string name, std::string current_place, int build_num, int window_h, int window_w, int viewport_h, int viewport_w) :
			Name(name), Current_place(current_place), Build_num(build_num), Window_width(window_w), Window_height(window_h), Viewport_height(viewport_h), Viewport_width(viewport_w) {}
		Game_project() = default;

		static Game_project from_json(const nlohmann::json j) {
			Game_project proj;
			j.at("Name").get_to(proj.Name);
			j.at("Start_place").get_to(proj.Current_place);
			j.at("num_of_builds").get_to(proj.Build_num);
			j.at("Window_Width").get_to(proj.Window_width);
			j.at("Window_Height").get_to(proj.Window_height);
			j.at("Viewport_Width").get_to(proj.Viewport_width);
			j.at("Viewport_Height").get_to(proj.Viewport_height);
			return proj;
		}

		NLOHMANN_DEFINE_TYPE_INTRUSIVE(Game_project, Name, Current_place, Build_num, Window_width, Window_height, Viewport_width, Viewport_height);

	};
}