#version 330
 
in  vec3 vPosition;
in  vec3 vNormal;
in  vec3 vColor;

out vec3 v_norm;
out vec4 color;
out vec3 v_pos;

uniform mat4 modelview;
uniform mat4 model;
uniform mat4 view;

 
void
main()
{
    gl_Position = modelview * vec4(vPosition, 1.0);
    v_norm = normalize(mat3(modelview) * vNormal);
    v_norm = vNormal;
	color = vec4( vColor, 1.0);


	mat3 normMatrix = transpose(inverse(mat3(model)));
	v_norm = normMatrix * vNormal;
    v_pos = (model * vec4(vPosition, 1.0)).xyz;
}
 