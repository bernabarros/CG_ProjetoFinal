#version 330 core

layout (location = 0) out vec3 gNormal;

in vec3 vNormal;

void main()
{
    gNormal = normalize(vNormal);
}
