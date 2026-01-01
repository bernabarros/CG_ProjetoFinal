#version 330 core

//in vec2 TexCoords;
out vec4 FragColor;

uniform sampler2D gNormal;

void main()
{
    // Read normal from gBuffer
    //vec3 normal = texture(gNormal, TexCoords).rgb;

    // Convert from [-1,1] to [0,1] for display
    //normal = normalize(normal) * 0.5 + 0.5;

    vec3 v = texture(gNormal, vec2(0.5,0.5)).rgb;

    FragColor = vec4(v, 1.0);
}

