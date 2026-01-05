#version 330 core
out float FragColor;

in vec2 TexCoords;

uniform sampler2D screenDepthMap;
uniform mat4 projection;    

// The direction of the light source
uniform vec3 lightDirView;        

const int MAX_STEPS = 32;       // Max Steps
const float MAX_DIST = 2.5;     // Max Distance it goes before giving up
const float THICKNESS = 0.1;    
const float STEP_LENGTH = 0.05; // Size of Step


vec3 GetViewPos(vec2 uv) {
    float z = texture(screenDepthMap, uv).r;
    
    // Convert coordinates to Normalized
    vec4 clipSpacePosition = vec4(uv * 2.0 - 1.0, z * 2.0 - 1.0, 1.0);

    // Multiply by the inverse projection matrix
    vec4 viewSpacePosition = inverse(projection) * clipSpacePosition;

    return viewSpacePosition.xyz / viewSpacePosition.w;
}

void main()
{
    // The starting position
    vec3 rayOrigin = GetViewPos(TexCoords);
    
    vec3 rayDir = normalize(lightDirView);
    
    float shadow = 0.0;

    // Current position
    vec3 rayPos = rayOrigin;
    

    for(int i = 0; i < MAX_STEPS; ++i)
    {
        // Advance the ray forward
        rayPos += rayDir * STEP_LENGTH;
        
        if(distance(rayOrigin, rayPos) > MAX_DIST) break;

        vec4 rayProj = projection * vec4(rayPos, 1.0);
        rayProj.xy /= rayProj.w;
        vec2 rayUV = rayProj.xy * 0.5 + 0.5;

        // Compare Depths
        float depthSample = texture(screenDepthMap, rayUV).r;
        vec3 geometryPos = GetViewPos(rayUV);
        
        if(rayPos.z < geometryPos.z && abs(rayPos.z - geometryPos.z) < THICKNESS)
        {
            shadow = 1.0; // Sombra detectada
            break;
        }
    }

    // The final shadow value
    FragColor = shadow; 
}