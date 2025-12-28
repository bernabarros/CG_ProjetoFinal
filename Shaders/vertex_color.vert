#version 330 core
layout (location = 0) in vec3 position;
layout (location = 1) in vec4 color;
layout (location = 2) in vec3 normal;

out vec4 outColor;
out float AOFactor;

void main()
{
    outColor = color;
    gl_Position = vec4(position, 1.0);

    AOFactor = clamp(dot(normalize(normal), vec3(0,1,0)), 0.3, 1.0);
}
