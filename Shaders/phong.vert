#version 330 core
layout (location = 0) in vec3 position;
layout (location = 1) in vec3 normal;

uniform vec4 MaterialColor = vec4(1,1,0,1);
uniform mat4 MatrixClip;

out vec4 fragColor;
out float AOFactor;

void main()
{
    fragColor = MaterialColor;
    gl_Position = MatrixClip * vec4(position, 1.0);

    AOFactor = clamp(dot(normalize(normal), vec3(0,1,0)), 0.3, 1.0);
}
