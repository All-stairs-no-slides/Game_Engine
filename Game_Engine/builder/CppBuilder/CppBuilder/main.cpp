#include <iostream>
#include <fstream>
#include <filesystem>
#include <nlohmann/json.hpp>
#include"Game_Object.h"
// glad must come before glfw
#include<glad/glad.h>
#include<glfw3.h>
// gl mathematics
#include <glm/glm.hpp>
#include <glm/gtc/matrix_transform.hpp>
#include <glm/gtc/type_ptr.hpp>
#include "Shader_Utilities.h"

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
	
	// normalisation vector used to normalise coordinates within the screen
	glm::mat4 norm_vec = glm::ortho(0.0f, 800.0f, 600.0f, 0.0f, -1.0f, 1.0f);

	// allow for window resizing
	glfwSetFramebufferSizeCallback(window, framebuffer_size_callback);

	// rectangle vertpos's
	float verts[] = {
		0.5f, 0.5f, 0.0f,
		-0.5f, -0.5f, 0.0f,
		-0.5f, 0.5f, 0.0f,
		0.5f, -0.5f, 0.0f
	};

	

	// vertex data stored in a buffer to be moved to GPU
	unsigned int VBO;
	unsigned int VAO;

	glGenVertexArrays(1, &VAO);

	glGenBuffers(1, &VBO);


	glBindVertexArray(VAO);

	glBindBuffer(GL_ARRAY_BUFFER, VBO);

	glBufferData(GL_ARRAY_BUFFER, sizeof(verts), verts, GL_STATIC_DRAW);

	const char* frag_shader = "#version 330 core\n"
		"out vec4 FragColor;\n"
		"void main()\n"
		"{\n"
		"FragColor = vec4(1.0f, 0.5f, 0.2f, 1.0f);\n"
		"}\n";

	const char* vert_shader = "#version 330 core\n"
		"layout (location = 0) in vec3 aPos;\n"
		"void main()\n"
		"{\n"
		"   gl_Position = vec4(aPos.x, aPos.y, aPos.z, 1.0);\n"
		"}\0";

	Shader_utils::Shader Basic_Shader(vert_shader, frag_shader);


	// linking the attributes of the vertex to the vertex shader (so it reads the data correctly)
	// -------------------------------------------------------------------

	glVertexAttribPointer(0, 3, GL_FLOAT, GL_FALSE, 3 * sizeof(float), (void*)0);
	glEnableVertexAttribArray(0);

	// render loop
	while (!glfwWindowShouldClose(window))
	{
		Basic_Shader.use();
		//Basic_Shader.setFloat("someUniform", 1.0f);
		glBindVertexArray(VAO);
		glDrawArrays(GL_TRIANGLE_STRIP, 0, 4);
		glfwSwapBuffers(window);
		glfwPollEvents();
	}
	
	// exit app
	glfwTerminate();
	return 0;
}