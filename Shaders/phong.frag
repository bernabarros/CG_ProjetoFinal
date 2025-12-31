#version 330 core
in vec4 fragColor;
out vec4 OutputColor;

uniform vec3 AmbientLight;

void main()
{
    vec3 litColor = fragColor.rgb * AmbientLight;
    OutputColor = vec4(litColor, fragColor.a);
}




