#version 330 core

layout (location = 0) in vec3 position;
layout (location = 1) in vec3 normal;

uniform mat4 MatrixWorld;
uniform mat4 MatrixCamera;
uniform mat4 MatrixProjection;

out vec3 vNormal;

void main()
{
    vec4 worldPos = MatrixWorld * vec4(position, 1.0);
    vec4 viewPos = MatrixCamera * worldPos;

    vNormal = mat3(MatrixCamera * MatrixWorld) * normal;

    gl_Position = MatrixProjection * viewPos;
}