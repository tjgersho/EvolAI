#version 330
 
in vec3 v_norm;
in vec4 color;
out vec4 outputColor;
 
void
main()
{
  vec3 n = normalize(v_norm);
 
   outputColor = color;
}
