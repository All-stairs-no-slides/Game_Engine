#include <iostream>
#include <fstream>
#include <filesystem>
#include <nlohmann/json.hpp>
#include"Game_Object.h"
// glad must come before glfw
#include<glad/glad.h>
#include<glfw3.h>
using json = nlohmann::json;
using namespace std;

void framebuffer_size_callback(GLFWwindow* window, int width, int height)
{
	// resizing func
	glViewport(0, 0, width, height);
}

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

	// setup window
	glfwInit();
	glfwWindowHint(GLFW_CONTEXT_VERSION_MAJOR, 3);
	glfwWindowHint(GLFW_CONTEXT_VERSION_MINOR, 3);
	glfwWindowHint(GLFW_OPENGL_PROFILE, GLFW_OPENGL_CORE_PROFILE);

	
	// start window
	GLFWwindow* window = glfwCreateWindow(800, 600, "LearnOpenGL", NULL, NULL);
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
	
	// allow for window resizing
	glfwSetFramebufferSizeCallback(window, framebuffer_size_callback);

	// render loop
	while (!glfwWindowShouldClose(window))
	{
		glfwSwapBuffers(window);
		glfwPollEvents();
	}

	// exit app
	glfwTerminate();
	return 0;
}