#include <iostream>
#include <fstream>
#include <filesystem>
#include <nlohmann/json.hpp>
#include "Game_Object.h"
#include "Place.h"
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
#include <pybind11/pybind11.h>

namespace py = pybind11;

using json = nlohmann::json;

void framebuffer_size_callback(GLFWwindow* window, int width, int height)
{
	// resizing func
	glViewport(0, 0, width, height);
}

int main()
{
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
				std::cout << "Component Type: " << "hihihihihihihihihihihihihihihihhihihhihihihhhhhhhhhhhhhhhhhhhhh" << "\n";
			}

		}
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