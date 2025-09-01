#include<string>
#include <nlohmann/json.hpp>

namespace Game_project {
	class Game_project {
	public:
		std::string Name;
		std::string Start_place;
		int Build_num;

		Game_project(std::string name, std::string start_place, int build_num) : Name(name), Start_place(start_place), Build_num(build_num) {}
		Game_project() = default;

		static Game_project from_json(const nlohmann::json j) {
			Game_project proj;
			j.at("Name").get_to(proj.Name);
			j.at("Start_place").get_to(proj.Start_place);
			j.at("num_of_builds").get_to(proj.Build_num);

			return proj;
		}

		NLOHMANN_DEFINE_TYPE_INTRUSIVE(Game_project, Name, Start_place, Build_num);

	};
}