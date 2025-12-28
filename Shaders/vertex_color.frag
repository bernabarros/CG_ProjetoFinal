#version 330 core

in vec4 outColor;
in float AOFactor;

out vec4 FragColor;

uniform vec3 AmbientLight;

void main()
{
    vec3 ambient = outColor.rgb * AmbientLight * AOFactor;
    
    FragColor = vec4(ambient, outColor.a);
}

