#version 330 core

in vec4 outColor;

out vec4 FragColor;

uniform vec3 AmbientLight;

void main()
{
    vec3 ambient = outColor.rgb * AmbientLight;
    
    FragColor = vec4(ambient, outColor.a);
}