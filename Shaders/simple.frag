#version 330 core
out vec4 FragColor;

uniform vec3 AmbientLight;

void main()
{
    vec3 baseColor = vec3(1.0, 0.5, 0.2); // original color
    vec3 litColor = baseColor * AmbientLight;
    FragColor = vec4(litColor, 1.0);
}