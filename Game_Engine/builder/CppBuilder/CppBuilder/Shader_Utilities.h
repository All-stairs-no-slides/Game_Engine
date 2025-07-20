#pragma once

#ifndef SHADER_H
#define SHADER_H

#include <glad/glad.h> // include glad to get all the required OpenGL headers

#include <string>
#include <fstream>
#include <sstream>
#include <iostream>

#include <glm/glm.hpp>
#include <glm/gtc/type_ptr.hpp>

namespace Shader_utils {
    class Shader
    {
    public:
        // the program ID
        unsigned int ID;

        // constructor reads and builds the shader
        Shader(const char* vertexPath, const char* fragmentPath);
        // use/activate the shader
        void use();
        // utility uniform functions
        void setBool(const std::string& name, bool value) const;
        void setInt(const std::string& name, int value) const;
        void setFloat(const std::string& name, float value) const;
        void setMatrix4(const std::string& name, const glm::mat4& matrix) const;
        void SetVector3f(const char* name, const glm::vec3& value) const;
    };
}
#endif