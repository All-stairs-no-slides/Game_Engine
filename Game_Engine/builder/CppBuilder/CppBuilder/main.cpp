#include <iostream>
#include <fstream>
#include <filesystem>
#include <nlohmann/json.hpp>
#include "Game_Object.h"
#include "Place.h"
#include "Game_project.h"
// glad must come before glfw
#include <glad/glad.h>
#include <glfw3.h>
// gl mathematics
#include <glm/glm.hpp>
#include <glm/gtc/matrix_transform.hpp>
#include <glm/gtc/type_ptr.hpp>
#include "Shader_Utilities.h"
#include "Textures.h"
// texture loading
#include "stb_image.h"
// python
#include <Python.h>
#include <pybind11/embed.h>

namespace py = pybind11;
using namespace py::literals;

using json = nlohmann::json;

void framebuffer_size_callback(GLFWwindow* window, int width, int height)
{
	// resizing func
	glViewport(0, 0, width, height);
}

namespace gc = game_components;



int main()
{
	py::scoped_interpreter guard{}; // start the interpreter and keep it alive
	//py::object scope = py::module_::import("__main__").attr("__dict__");

	py::module_ sys = py::module_::import("sys");
	sys.attr("path").attr("append")(R"(C:\Users\amcd1\Desktop\projects\Game_Engine\tests\Scripts)");
	//py::eval_file(R"(C:\Users\amcd1\Desktop\projects\Game_Engine\tests\Scripts\Test_1.py)", scope);

	py::module_ mymodule = py::module_::import("Test_1");

	// Create a dummy module at runtime
	auto main_module = py::module_::import("__main__");

	// register the class
	py::class_<gc::Game_Component, std::shared_ptr<gc::Game_Component>>(main_module, "Game_Component")
		.def(py::init<>())
		.def_readwrite("type", &gc::Game_Component::type);

	// setup window
	glfwInit();
	glfwWindowHint(GLFW_CONTEXT_VERSION_MAJOR, 3);
	glfwWindowHint(GLFW_CONTEXT_VERSION_MINOR, 3);
	glfwWindowHint(GLFW_OPENGL_PROFILE, GLFW_OPENGL_CORE_PROFILE);

	
	// start window
	GLFWwindow* window = glfwCreateWindow(800, 600, "placeholder, remember to fix dumbass", NULL, NULL);
	if (window == NULL)
	{
		std::cout << "Failed to create GLFW window" << std::endl;
		glfwTerminate();
		return -1;
	}

	// init glfw
	glfwMakeContextCurrent(window);

	if (!gladLoadGLLoader((GLADloadproc)glfwGetProcAddress))
	{
		std::cout << "Failed to initialize GLAD" << std::endl;
		return -1;
	}

	

	// set viewport
	glViewport(0, 0, 800, 600);

	// transparency
	glEnable(GL_BLEND);
	glBlendFunc(GL_SRC_ALPHA, GL_ONE_MINUS_SRC_ALPHA);
	
	// normalisation vector used to normalise coordinates within the screen
	glm::mat4 norm_vec = glm::ortho(0.0f, 800.0f, 600.0f, 0.0f, -1.0f, 1.0f);

	// allow for window resizing
	glfwSetFramebufferSizeCallback(window, framebuffer_size_callback);

	// load project
	Game_project::Game_project project;
	bool project_found = false;
	std::string path = R"(C:\Users\amcd1\Desktop\projects\Game_Engine\tests)";
	for (auto f : std::filesystem::directory_iterator(path)) {
		//std::cout << f.path().filename().string() << std::endl;
		// find final suffix
		std::string p = f.path().filename().string();
		int suffix_innit = -1;
		for (int i = 0; i < p.length(); i++) {
			if (p[i] == '.') {
				suffix_innit = i;
			}
		}
		//std::cout << p.substr(suffix_innit) << std::endl;
		if (suffix_innit == -1) {
			continue;
		}
		if (p.substr(suffix_innit) == ".proj") {
			std::cout << f.path().string();
			std::ifstream file(f.path().string());
			json proj_json = json::parse(file);
			std::cout << "proj: " << proj_json << std::endl;
			project = project.from_json(proj_json);
			project_found = true;
			break;
		}
		
	 }

	if (project_found == false) {
		std::cerr << "there is no project file";
		throw;
	}
	

	

	

	// load places
	std::ifstream f(R"(C:\Users\amcd1\Desktop\projects\Game_Engine\tests\Places\ppp.place)");
	json plain_json = json::parse(f);
	std::cout << "Current path is: " << plain_json << std::endl;
	Place::Place place;
	place = place.from_json(plain_json);


	for (const auto& comp : place.Instances[0].components) {
		if (comp->type == "Sprite_renderer") {
			std::shared_ptr<game_components::sprite_renderer> spr_renderer = std::dynamic_pointer_cast<game_components::sprite_renderer>(comp);
			if (spr_renderer) {
				spr_renderer->Initialisation();
			}

		}
	}

	// Get the class
	py::object MyClass = mymodule.attr("test");

	// Create an instance
	py::object instance = MyClass();
	// check that the file both has the intended method name and that it is indeed a function
	if (py::hasattr(instance, "hi")) {
		py::object method = instance.attr("hi");

		// Confirm it's actually callable
		if (py::isinstance<py::function>(method)) {
			std::cout << "the method exists and is callable.\n";
			//py::function olla = instance["hi"];
			//std::shared_ptr<gc::transform_component> Trans = std::dynamic_pointer_cast<gc::transform_component>(place.Instances[0].components[0]);
			auto Trans = std::make_shared<gc::Game_Component>();
			Trans->type = "me";
			auto casted = py::cast(Trans);
			if (!casted) {
				std::cerr << "py::cast returned null\n";
				return 0;
			}
			std::cout << "Python sees:" << py::str(casted).cast<std::string>() << std::endl;
			try {
				method(casted);
			}
			catch (py::cast_error e) {
				std::cout << "fuck " << e.what() << std::endl;
			}

		}
		else {
			std::cout << "the method exists but is not callable.\n";
		}
	}
	else {
		std::cout << "the method does NOT exist.\n";
	}
	
	// Game loop
	while (!glfwWindowShouldClose(window))
	{
		// clear screen
		glClearColor(0.1f, 0.1f, 0.1f, 1.0f); 
		glClear(GL_COLOR_BUFFER_BIT);
		game_components::transform_component current_transform;
		// render loop
		for (const auto& comp : place.Instances[0].components) {

			if (comp->type == "Transform") {
				std::shared_ptr<game_components::transform_component> Transform = std::dynamic_pointer_cast<game_components::transform_component>(comp);

				if (Transform) {
					current_transform = *Transform.get();
				}
				continue;

			}
			if (comp->type == "Sprite_renderer") {
				std::shared_ptr<game_components::sprite_renderer> spr_renderer = std::dynamic_pointer_cast<game_components::sprite_renderer>(comp);

				if (spr_renderer) {

					//std::cout << spr_renderer->Sprite_dir << std::endl;

					/*std::cout << current_transform.x_scale << std::endl;
					std::cout << current_transform.y_scale << std::endl;

					std::cout << "renderer" << std::endl;

					std::cout << spr_renderer->x_scale << std::endl;
					std::cout << spr_renderer->y_scale << std::endl;*/

					spr_renderer->DrawSelf(
						glm::vec2((float)current_transform.x + (float)spr_renderer->x_offset, (float)current_transform.y + (float)spr_renderer->y_offset), // position
						glm::vec2(current_transform.x_scale * spr_renderer->x_scale, current_transform.y_scale * spr_renderer->y_scale), // scale
						current_transform.rotation + spr_renderer->rotation, // rotation
						glm::vec3(1.0f, 1.0f, 1.0f)); // colour
				}
				continue;

			}
		}
	
		glfwSwapBuffers(window);
		glfwPollEvents();
	}
	
	// exit app
	glfwTerminate();
	return 0;
}