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
#include <pybind11/stl.h>
#include <pybind11/stl_bind.h>


namespace py = pybind11;
using namespace py::literals;

using json = nlohmann::json;

void framebuffer_size_callback(GLFWwindow* window, int width, int height)
{
	// resizing func
	glViewport(0, 0, width, height);
}

namespace gc = game_components;


PYBIND11_EMBEDDED_MODULE(engine, m) {
	// register the classes
	py::class_<gc::Game_Component, std::shared_ptr<gc::Game_Component>>(m, "Game_Component")
		.def(py::init<>())
		.def_readonly("type", &gc::Game_Component::type);

	py::class_<gc::sprite_renderer, gc::Game_Component, std::shared_ptr<gc::sprite_renderer>>(m, "sprite_renderer")
		.def(py::init<>())
		.def_readonly("type", &gc::sprite_renderer::type)
		.def_readwrite("x_offset", &gc::sprite_renderer::x_offset)
		.def_readwrite("y_offset", &gc::sprite_renderer::y_offset)
		.def_readwrite("x_scale", &gc::sprite_renderer::x_scale)
		.def_readwrite("y_scale", &gc::sprite_renderer::y_scale)
		.def_readwrite("rotation", &gc::sprite_renderer::rotation)
		.def_readwrite("depth", &gc::sprite_renderer::depth)
		.def_readwrite("shader", &gc::sprite_renderer::shader)
		.def_readwrite("sprite_dir", &gc::sprite_renderer::Sprite_dir);

	//py::bind_vector<std::vector<std::shared_ptr<gc::Game_Component>>>(m, "ComponentVector");

	py::class_<game_object::Game_Object>(m, "Game_Object")
		.def(py::init<>())
		.def_readwrite("name", &game_object::Game_Object::Name)
		.def_readwrite("components", &game_object::Game_Object::components);

	py::class_<Place::Place>(m, "Place")
		.def(py::init<>())
		.def_readwrite("place_Name", &Place::Place::Place_name)
		.def_readwrite("instances", &Place::Place::Instances);
}
int main()
{


	//============================================================
	// Window stuff
	//============================================================


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
	//glm::mat4 norm_vec = glm::ortho(0.0f, 800.0f, 600.0f, 0.0f, -1.0f, 1.0f);

	// allow for window resizing
	glfwSetFramebufferSizeCallback(window, framebuffer_size_callback);


	//============================================================
	// load project
	//============================================================

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


	
	//============================================================
	// SCRIPTING
	//============================================================

	py::scoped_interpreter guard{}; // start the interpreter and keep it alive

	py::module_ sys = py::module_::import("sys");
	sys.attr("path").attr("append")(R"(C:\Users\amcd1\Desktop\projects\Game_Engine\tests\Scripts)");

	py::module_ engine = py::module_::import("engine");

	py::module_ mymodule;
	
	//============================================================
	// load instances from initial place
	//============================================================

	std::ifstream f(R"(C:\Users\amcd1\Desktop\projects\Game_Engine\tests\Places\)" + project.Current_place + ".place");
	json plain_json = json::parse(f);
	std::cout << "Current path is: " << plain_json << std::endl;
	Place::Place place;
	// IMPORTANT NOTE: initialisaion functions for individual components are performed upon initialisation inside their constructors
	place = place.from_json(plain_json);

	
	//============================================================
	// Game loop
	//============================================================
	
	while (!glfwWindowShouldClose(window))
	{

		 //clear screen
		glClearColor(0.1f, 0.1f, 0.1f, 1.0f); 
		glClear(GL_COLOR_BUFFER_BIT);

		// component loops (based on the order they show up)
		for (game_object::Game_Object g_obj : place.Instances) {
			// includes transforms sprite renderers and scripts
			g_obj.Components_Loop(&place);
		}

	
		glfwSwapBuffers(window);
		glfwPollEvents();
	}
	
	// exit app
	glfwTerminate();

	return 0;
}