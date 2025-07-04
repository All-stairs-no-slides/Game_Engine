#include <iostream>
#include <fstream>
#include <filesystem>
#include <nlohmann/json.hpp>
#include "Game_Object.h"
// glad must come before glfw
#include <glad/glad.h>
#include <glfw3.h>
// gl mathematics
#include <glm/glm.hpp>
#include <glm/gtc/matrix_transform.hpp>
#include <glm/gtc/type_ptr.hpp>
#include "Shader_Utilities.h"
// texture loading
#include "stb_image.h"

using json = nlohmann::json;






void framebuffer_size_callback(GLFWwindow* window, int width, int height)
{
	// resizing func
	glViewport(0, 0, width, height);
}

int main()
{

	std::ifstream f(R"(C:\Users\amcd1\Desktop\projects\Game_Engine\tests\Objects\nnn\nnn.obj)");
	json plain_json = json::parse(f);
	std::cout << "Current path is: " << plain_json << std::endl;
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

	unsigned int texture;
	glGenTextures(1, &texture);
	glBindTexture(GL_TEXTURE_2D, texture);
	// set the texture wrapping/filtering options (on the currently bound texture object)
	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, GL_REPEAT);
	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, GL_REPEAT);
	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR_MIPMAP_LINEAR);
	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_LINEAR);
	// load image
	stbi_set_flip_vertically_on_load(true);
	// 
	// width height and colour channels
	int width, height, nrChannels;
	unsigned char* data = stbi_load("C:\\Users\\amcd1\\Desktop\\projects\\Game_Engine\\tests\\Assets\\scrn.png", &width, &height, &nrChannels, 0);
	if (data)
	{
		glTexImage2D(GL_TEXTURE_2D, 0, GL_RGBA, width, height, 0, GL_RGBA, GL_UNSIGNED_BYTE, data);
		glGenerateMipmap(GL_TEXTURE_2D);
	}
	else
	{
		std::cout << "Failed to load texture" << std::endl;
	}
	stbi_image_free(data);

	// rectangle vertpos's

	float verts[] = {
		// positions          // colors           // texture coords
		 0.5f,  0.5f, 0.0f,   1.0f, 0.0f, 0.0f,   1.0f, 1.0f,   // top right
		 0.5f, -0.5f, 0.0f,   0.0f, 1.0f, 0.0f,   1.0f, 0.0f,   // bottom right
		-0.5f, -0.5f, 0.0f,   0.0f, 0.0f, 1.0f,   0.0f, 0.0f,   // bottom left
		-0.5f,  0.5f, 0.0f,   1.0f, 1.0f, 0.0f,   0.0f, 1.0f    // top left 
	};
	unsigned int indices[] = {  // note that we start from 0!
		0, 1, 3,   // first triangle
		1, 2, 3    // second triangle
	};

	

	// vertex data stored in a buffer to be moved to GPU
	unsigned int VBO;
	unsigned int VAO;
	unsigned int EBO;

	// setup vertex array object
	glGenVertexArrays(1, &VAO);
	glBindVertexArray(VAO);

	// setup vertex buffer object, and store verticies in buffer so opengl can use
	glGenBuffers(1, &VBO);
	glBindBuffer(GL_ARRAY_BUFFER, VBO);
	glBufferData(GL_ARRAY_BUFFER, sizeof(verts), verts, GL_STATIC_DRAW);

	// setup element buffer array so that the verticies that make up each individual triangle can be stored in a buffer and used by opengl
	glGenBuffers(1, &EBO);
	glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, EBO);
	glBufferData(GL_ELEMENT_ARRAY_BUFFER, sizeof(indices), indices, GL_STATIC_DRAW);


	// linking the attributes of the vertex to the vertex shader (so it reads the data correctly)
	// -------------------------------------------------------------------
	// position attribute
	glVertexAttribPointer(0, 3, GL_FLOAT, GL_FALSE, 8 * sizeof(float), (void*)0);
	glEnableVertexAttribArray(0);

	// color attribute (optional if not using color)
	glVertexAttribPointer(1, 3, GL_FLOAT, GL_FALSE, 8 * sizeof(float), (void*)(3 * sizeof(float)));
	glEnableVertexAttribArray(1);

	// the 
	glVertexAttribPointer(2, 2, GL_FLOAT, GL_FALSE, 8 * sizeof(float), (void*)(6 * sizeof(float)));
	glEnableVertexAttribArray(2);

	const char* vert_shader = "#version 330 core\n"
		"layout(location = 0) in vec3 aPos;\n"
		"layout(location = 1) in vec3 aColor; \n"
		"layout(location = 2) in vec2 aTexCoord; \n"

		"out vec3 ourColor; \n"
		"	out vec2 TexCoord; \n"
		"void main()\n"
		"{ \n"
		"		gl_Position = vec4(aPos, 1.0); \n"
		"ourColor = aColor; \n"
		"	TexCoord = aTexCoord; \n"
	"}";

	const char* frag_shader = "#version 330 core\n"
		"out vec4 FragColor;\n"
		"in vec3 ourColor;\n"
		"in vec2 TexCoord;\n"
		"uniform sampler2D ourTexture;\n"

		"void main()\n"
		"{\n"
		"FragColor = texture(ourTexture, TexCoord);\n"
		"}\n";

	Shader_utils::Shader Basic_Shader(vert_shader, frag_shader);

	Basic_Shader.use(); // don't forget to activate the shader before setting uniforms!  
	//glUniform1i(glGetUniformLocation(Basic_Shader.ID, "ourTexture"), 0); // set it manually

	

	// render loop
	while (!glfwWindowShouldClose(window))
	{

		Basic_Shader.use();
		//glActiveTexture(GL_TEXTURE0); // activate the texture unit first before binding texture
		glBindTexture(GL_TEXTURE_2D, texture);
		glBindVertexArray(VAO);
		glDrawElements(GL_TRIANGLES, 6,GL_UNSIGNED_INT, 0);
		glfwSwapBuffers(window);
		glfwPollEvents();
	}
	
	// exit app
	glfwTerminate();
	return 0;
}