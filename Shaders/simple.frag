#version 330 core
in float AOFactor;
out vec4 FragColor;

uniform vec3 AmbientLight;

void main()
{
    vec3 baseColor = vec3(1.0, 0.5, 0.2); 
    vec3 litColor = baseColor * AmbientLight * AOFactor;
    FragColor = vec4(litColor, 1.0);
}