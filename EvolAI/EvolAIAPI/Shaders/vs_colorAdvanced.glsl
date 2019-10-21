#version 330

in vec3 vPosition;
in vec3 vNormal;
in vec3 vColor;

out vec3 v_norm;
out vec3 v_pos;
out vec3 v_color;

uniform mat4 modelview;
uniform mat4 model;
uniform mat4 view;

void
main()
{
 gl_Position = modelview * vec4(vPosition, 1.0);
 v_color = vColor;

 mat3 normMatrix = transpose(inverse(mat3(model)));
 v_norm = normMatrix * vNormal;
 v_pos = (model * vec4(vPosition, 1.0)).xyz;
}