#include "Game_Component.h"
#include <iostream>
#include"Sprite_Asset.h"
#include<fstream>

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

// audio libraries
#include <miniaudio/miniaudio.h>

// project libraries
#include "Game_Object.h"
#include "Place.h"



using namespace game_components;
using json = nlohmann::json;

//-------------------------------------------------
// AUDIO COMPONENT
//-------------------------------------------------

void audio_component::Play()
{
    ma_sound_start(this->sound.get());
    ma_sound_set_pitch(this->sound.get(), 2.0);
}

void audio_component::Set_pitch(float pitch)
{
    ma_sound_set_pitch(this->sound.get(), pitch);
}

void audio_component::Pause()
{
    ma_sound_stop(this->sound.get());
}

void audio_component::Set_start_time_mill(ma_uint64 time) {
    ma_sound_set_start_time_in_milliseconds(this->sound.get(), time);
}

ma_uint64 audio_component::Get_time_mil() {
    return ma_engine_get_time_in_milliseconds(this->s_engine.get());
}

void audio_component::Initialisation()
{
    ma_result result;
    ma_engine eng;
    ma_sound s;

    this->s_engine = std::make_shared<ma_engine>(eng);
    this->sound = std::make_shared<ma_sound>(s);

    result = ma_engine_init(NULL, this->s_engine.get());
    assert(result == MA_SUCCESS);

    result = ma_sound_init_from_file(this->s_engine.get(), this->path.c_str(), 0, NULL, NULL, this->sound.get());
    assert(result == MA_SUCCESS);

}


//-------------------------------------------------
// SCRIPT COMPONENT OVERLOADS
//-------------------------------------------------
void script_component::Initialisation() 
{
    std::cout << "initialising script component" << std::endl;
    std::string copied_path = this->path;
    std::cout << copied_path << std::endl;
    copied_path.erase(copied_path.find("."));
    this->script_module = py::module_::import(copied_path.c_str());
}

void script_component::Event_Call(const char* event_name, game_object::Game_Object* parsed_item, py::module_ engine_api, Place::User_Inputs* User_Inputs) {
    py::module_ mymodule = this->script_module;
    py::object Instance_api = engine_api.attr("Instance");
    py::object Inputs_api = engine_api.attr("User_Inputs");


    // remove the .py suffix from the file name
    std::string script_name = this->path.substr(0, this->path.length() - 3);
    
    // Get the class
    py::object MyClass = mymodule.attr(script_name.c_str());

    // Create an instance
    py::object instance = MyClass();
    // check that the file both has the intended method name and that it is indeed a function
    if (py::hasattr(instance, event_name)) {
        py::object method = instance.attr(event_name);

        // Confirm it's actually callable
        if (py::isinstance<py::function>(method)) {
            //std::cout << "the method exists and is callable.\n";

            auto casted = py::cast(parsed_item);
            auto casted_inputs = py::cast(User_Inputs);
            if (!casted) {
                std::cerr << "py::cast returned null\n";
                return;
            }
            try {
                method(Instance_api(casted), Inputs_api(casted_inputs));

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
}

void script_component::Event_Call(const char* event_name, game_object::Game_Object* this_obj, game_components::Contact* collsion, py::module_ engine_api, Place::User_Inputs* User_Inputs) {
    py::module_ mymodule = this->script_module;
    py::object Contact_api = engine_api.attr("Contact");
    py::object Instance_api = engine_api.attr("Instance");
    py::object Inputs_api = engine_api.attr("User_Inputs");



    // remove the .py suffix from the file name
    std::string script_name = this->path.substr(0, this->path.length() - 3);

    // Get the class
    py::object MyClass = mymodule.attr(script_name.c_str());

    // Create an instance
    py::object instance = MyClass();
    // check that the file both has the intended method name and that it is indeed a function
    if (py::hasattr(instance, event_name)) {
        py::object method = instance.attr(event_name);

        // Confirm it's actually callable
        if (py::isinstance<py::function>(method)) {
            //std::cout << "the method exists and is callable.\n";

            auto this_casted = py::cast(this_obj);
            auto collided_casted = py::cast(collsion);
            auto casted_inputs = py::cast(User_Inputs);

            if (!this_casted || !collided_casted) {
                std::cerr << "py::cast returned null\n";
                return;
            }
            try {
                method(Instance_api(this_casted), Contact_api(collided_casted), Inputs_api(casted_inputs));

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
}

void script_component::Event_Call(const char* event_name, Place::Place* parsed_item, py::module_ engine_api, Place::User_Inputs* User_Inputs) {
    py::module_ mymodule = this->script_module;
    py::object place_api = engine_api.attr("Place");
    py::object Inputs_api = engine_api.attr("User_Inputs");

    // remove the .py suffix from the file name
    std::string script_name = this->path.substr(0, this->path.length() - 3);

    // Get the class
    py::object MyClass = mymodule.attr(script_name.c_str());

    // Create an instance
    py::object instance = MyClass();
    // check that the file both has the intended method name and that it is indeed a function
    if (py::hasattr(instance, event_name)) {
        py::object method = instance.attr(event_name);

        // Confirm it's actually callable
        if (py::isinstance<py::function>(method)) {
            //std::cout << "the method exists and is callable.\n";

            auto casted = py::cast(parsed_item);
            auto casted_inputs = py::cast(User_Inputs);

            if (!casted) {
                std::cerr << "py::cast returned null\n";
                return;
            }
            try {

                method(place_api(casted), Inputs_api(casted_inputs));

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
}


//-------------------------------------------------
// SPRITE RENDERER OVERLOADS
//-------------------------------------------------
void sprite_renderer::Initialisation() 
{
    std::cout << "initialising sprite renderer component" << std::endl;
    glm::mat4 projection = glm::ortho(0.0f, static_cast<float>(800),
        static_cast<float>(600), 0.0f, -1.0f, 1.0f);
    this->shader.use();
    this->shader.setInt("image", 0);
    this->shader.setMatrix4("projection", projection);
	// load sprites json
	std::ifstream f(this->Sprite_dir);
	json plain_json = json::parse(f);
	// deserialize sprite
	Sprite::Sprite spr;
	plain_json.at("name").get_to(spr.Name);
	plain_json.at("durations").get_to(spr.durations);
	plain_json.at("Images_location").get_to(spr.Image_location);
    spr.Initialise();
	this->sprite = spr;

	// initialise render data

    // configure VAO/VBO
    unsigned int VBO;
    float vertices[] = {
        // pos      // tex
        0.0f, 1.0f, 0.0f, 1.0f,
        1.0f, 0.0f, 1.0f, 0.0f,
        0.0f, 0.0f, 0.0f, 0.0f,

        0.0f, 1.0f, 0.0f, 1.0f,
        1.0f, 1.0f, 1.0f, 1.0f,
        1.0f, 0.0f, 1.0f, 0.0f
    };

    glGenVertexArrays(1, &this->VAO);
    glGenBuffers(1, &VBO);

    glBindBuffer(GL_ARRAY_BUFFER, VBO);
    glBufferData(GL_ARRAY_BUFFER, sizeof(vertices), vertices, GL_STATIC_DRAW);

    glBindVertexArray(this->VAO);
    glEnableVertexAttribArray(0);
    glVertexAttribPointer(0, 4, GL_FLOAT, GL_FALSE, 4 * sizeof(float), (void*)0);
    glBindBuffer(GL_ARRAY_BUFFER, 0);
    glBindVertexArray(0);
}


void sprite_renderer::DrawSelf(glm::vec2 position,
    glm::vec2 scale, float rotate, glm::vec3 color)
{
    // prep texture
    glActiveTexture(GL_TEXTURE0);
    Textures::Texture2D tex = this->sprite.Get_Current_texture();
    tex.Bind();

    glm::vec2 size = glm::vec2(tex.Width * scale.x, tex.Height * scale.y);

    //std::cout << tex.Width << std::endl << tex.Height << std::endl;
    // prepare transformations
    this->shader.use();
    glm::mat4 model = glm::mat4(1.0f);
    model = glm::translate(model, glm::vec3(position, 0.0f));

    model = glm::translate(model, glm::vec3(0.5f * size.x, 0.5f * size.y, 0.0f));
    model = glm::rotate(model, glm::radians(rotate), glm::vec3(0.0f, 0.0f, 1.0f));
    model = glm::translate(model, glm::vec3(-0.5f * size.x, -0.5f * size.y, 0.0f));

    model = glm::scale(model, glm::vec3(size, 1.0f));

    this->shader.setMatrix4("model", model);
    this->shader.SetVector3f("spriteColor", color);

    

    glBindVertexArray(this->VAO);
    glDrawArrays(GL_TRIANGLES, 0, 6);
    glBindVertexArray(0);
}