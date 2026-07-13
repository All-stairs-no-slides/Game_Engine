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
// audio libraries
#include <miniaudio/miniaudio.h>
// python
#include <Python.h>
#include <pybind11/embed.h>
#include <pybind11/stl.h>
#include <pybind11/stl_bind.h>
// dubugging
#include <assert.h>


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

	py::class_<gc::transform_component, gc::Game_Component, std::shared_ptr<gc::transform_component>>(m, "transform")
		.def(py::init<>())
		.def_readonly("type", &gc::transform_component::type)
		.def_readwrite("x", &gc::transform_component::x)
		.def_readwrite("y", &gc::transform_component::y)
		.def_readwrite("z", &gc::transform_component::z)
		.def_readwrite("x_scale", &gc::transform_component::x_scale)
		.def_readwrite("y_scale", &gc::transform_component::y_scale)
		.def_readwrite("rotation", &gc::transform_component::rotation);

	py::class_<gc::Collider, gc::Game_Component, std::shared_ptr<gc::Collider>>(m, "Collider")
		.def(py::init<>())
		.def_readonly("type", &gc::Collider::type)
		.def_readwrite("Collider_type", &gc::Collider::Collider_type)
		.def_readwrite("Collider_alias", &gc::Collider::Collider_alias)
		.def_readwrite("Proportions", &gc::Collider::Proportions);

	py::class_<gc::audio_component, gc::Game_Component, std::shared_ptr<gc::audio_component>>(m, "Audio")
		.def(py::init<>())
		.def("Play", &gc::audio_component::Play)
		.def("Get_time_mil", &gc::audio_component::Get_time_mil)
		.def("Set_time_sec", &gc::audio_component::Set_time_sec)
		.def("Pause", &gc::audio_component::Pause)
		.def("Set_pitch", &gc::audio_component::Set_pitch)
		.def_readwrite("type", &gc::audio_component::type)
		.def_readwrite("sound_alias", &gc::audio_component::sound_alias);

	//py::bind_vector<std::vector<std::shared_ptr<gc::Game_Component>>>(m, "ComponentVector");

	py::class_<game_object::Game_Object>(m, "Game_Object")
		.def(py::init<>())
		.def_readwrite("name", &game_object::Game_Object::Name)
		.def_readwrite("components", &game_object::Game_Object::components)
		.def_readwrite("Locals", &game_object::Game_Object::Locals);

	py::class_<Place::Place>(m, "Place")
		.def(py::init<>())
		.def("Instantiate", &Place::Place::Instantiate)
		.def_readwrite("place_Name", &Place::Place::Place_name)
		.def_readwrite("next_place", &Place::Place::Next_place_name)
		.def_readwrite("instances", &Place::Place::Instances)
		.def_readwrite("Globals", &Place::Place::Globals);


	py::class_<game_components::Contact>(m, "Contact")
		.def(py::init<>())
		.def_readwrite("obj_1", &game_components::Contact::obj_1)
		.def_readwrite("obj_2", &game_components::Contact::obj_2)
		.def_readwrite("col_1", &game_components::Contact::col_1)
		.def_readwrite("col_2", &game_components::Contact::col_2);

	py::class_<Place::User_Inputs>(m, "User_Input")
		.def(py::init<>())
		.def_readwrite("mousex", &Place::User_Inputs::mousex)
		.def_readwrite("mousey", &Place::User_Inputs::mousey)
		.def_readwrite("L_mouse", &Place::User_Inputs::L_mouse_pressed)
		.def_readwrite("R_mouse", &Place::User_Inputs::R_mouse_pressed)
		.def_readwrite("Keys_pressed", &Place::User_Inputs::pressed_keys);

	



}
void Set_keys(GLFWwindow* window, Place::User_Inputs* In) {
	// takes inputs for keys and mice and pipes them into the inputs variable

	// reminde: expand keys checked later (to things like function and foreighn keys)
	for (int i = 0; i < 32; i++) {
		if (glfwGetKey(window, i) == GLFW_PRESS) {
			In->pressed_keys.push_back(i);
		}
	}
	if (glfwGetMouseButton(window, GLFW_MOUSE_BUTTON_LEFT) == GLFW_PRESS) {
		In->L_mouse_pressed = true;
	}
	else {
		In->L_mouse_pressed = false;

	}

	if (glfwGetMouseButton(window, GLFW_MOUSE_BUTTON_RIGHT) == GLFW_PRESS) {
		In->R_mouse_pressed = true;
	}
	else {
		In->R_mouse_pressed = false;

	}
};

int main()
{

	//============================================================
	// load project
	//============================================================

	Game_project::Game_project project;
	bool project_found = false;
	//std::string path = std::filesystem::current_path().string();
	const std::string path = R"(C:\Users\amcd1\Desktop\projects\Game_Engine\tests)";
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
	// Window stuff
	//============================================================


	// setup window
	glfwInit();
	glfwWindowHint(GLFW_CONTEXT_VERSION_MAJOR, 3);
	glfwWindowHint(GLFW_CONTEXT_VERSION_MINOR, 3);
	glfwWindowHint(GLFW_OPENGL_PROFILE, GLFW_OPENGL_CORE_PROFILE);

	
	// start window
	GLFWwindow* window = glfwCreateWindow(project.Window_width, project.Window_height, project.Name.c_str(), NULL, NULL);
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
	glViewport(0, 0, project.Viewport_width, project.Viewport_height);

	// transparency
	glEnable(GL_BLEND);
	glBlendFunc(GL_SRC_ALPHA, GL_ONE_MINUS_SRC_ALPHA);
	
	// normalisation vector used to normalise coordinates within the screen
	//glm::mat4 norm_vec = glm::ortho(0.0f, 800.0f, 600.0f, 0.0f, -1.0f, 1.0f);

	// allow for window resizing
	//glfwSetFramebufferSizeCallback(window, framebuffer_size_callback);


	


	
	//============================================================
	// SCRIPTING
	//============================================================

	py::scoped_interpreter guard{}; // start the interpreter and keep it alive

	py::module_ sys = py::module_::import("sys");

	sys.attr("path").attr("append")(path + "\\Scripts");
	//sys.attr("path").attr("append")(R"(C:\Users\amcd1\Desktop\projects\Game_Engine\Game_Engine\engine_api)");

	py::module_ engine = py::module_::import("engine");
	py::module_ engine_api = py::module_::import("engine_api");
	
	//============================================================
	// load instances from initial place
	//============================================================
	if (project.Current_place == "") {
		std::cerr << "initial place not set";
		throw;
	}

	std::ifstream f(path + "\\Places\\" + project.Current_place + ".place");
	json plain_json = json::parse(f);
	std::cout << "Current path is: " << plain_json << std::endl;
	Place::Place place;
	// IMPORTANT NOTE: initialisaion functions for individual components are performed upon initialisation inside their constructors
	place = place.from_json(plain_json, path);



	// ==========================================================
	// Initialise Instantiable objects
	// ==========================================================

	std::vector<game_object::Game_Object> Instantiables;

	if (!std::filesystem::is_directory(path + "\\Objects"))
	{
		std::cerr << "no objects dir" << std::endl;
		return 0;
	}

	std::filesystem::directory_iterator di = std::filesystem::directory_iterator(path + "\\Objects");
	for (std::filesystem::path p : di) {
		std::string str_p = p.string();
		int last_p = str_p.find_last_of("\\");
		std::string file_name = str_p.substr(last_p, str_p.length());
		p = std::filesystem::path(str_p + file_name + ".obj");
		game_object::Game_Object obj;
		std::ifstream f(p.string());
		json plain_json = json::parse(f);
		obj = obj.from_json(plain_json, path);
		Instantiables.push_back(obj);
	}
	
	
	//============================================================
	// Game loop
	//============================================================
	// 
	//int iters = 1;
	while (!glfwWindowShouldClose(window))
	{
		
		// maintain a local context so it refreshes per frame
		Place::User_Inputs User_In;
		// get user inputs
		glfwGetCursorPos(window, &User_In.mousex, &User_In.mousey);
		//std::cout << User_In.mousex << ", " << User_In.mousey << std::endl;

		Set_keys(window, &User_In);
		
		 //clear screen
		glClearColor(0.1f, 0.1f, 0.1f, 1.0f); 
		glClear(GL_COLOR_BUFFER_BIT);

		// collision registration loop
		place.Collider_Loop();
		
		//std::cout << iters << std::endl;
		//iters += 1;
		// component loops (based on the order they show up)
		for (int i = 0; i < place.Instances.size(); i++) {
			// includes transforms sprite renderers and scripts
			place.Instances[i].Components_Loop(&place, engine_api, &User_In);
		}

		// process the instantiated objects
		place.Process_Instantiation_Queue(&Instantiables);
			
		// change place if there has been a change
		if (place.Next_place_name != "") {
			std::ifstream f(path + "\\Places\\" + place.Next_place_name + ".place");
			json plain_json = json::parse(f);
			std::cout << "Current path is: " << plain_json << std::endl;
			place = place.from_json(plain_json, path);
			
		}

	
		glfwSwapBuffers(window);
		glfwPollEvents();
	}
	
	// exit app
	glfwTerminate();

	return 0;
}