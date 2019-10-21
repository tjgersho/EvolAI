using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System.Drawing;
using GodAIAPI.BuildingBlocks;

namespace GodAIAPI.UI
{
    // This is where all OpenGL code will be written.
    // OpenTK allows for several functions to be overriden to extend functionality; this is how we'll be writing code.

    // A simple constructor to let us set the width/height/title of the window.
    public class Window : GameWindow
    {
        public Window(Universe universe) : base(512, 512, new GraphicsMode(32, 24, 0, 4))
        {
            _universe = universe;
            _universe.ParticleAdded += NewParticleInTheUniverse;
        }

        private void NewParticleInTheUniverse(object sender, ParticleAddedEvent e)
        {
            lock (objects)
            {
                if (textures.ContainsKey("earth.png") && materials.ContainsKey("earth"))
                {
                    //e.NewParticle.TextureID = textures["earth.png"];
                    //e.NewParticle.Material = materials["earth"];
                   // e.NewParticle.LoadFromAnotherObjVolume(SpherObjVol);
                }
                objects.Add(e.NewParticle);
            }
        }

        private readonly Universe _universe;
        
        Dictionary<string, ShaderProgram> shaders = new Dictionary<string, ShaderProgram>();
        string activeShader = "colorAdvanced"; // "normal";// "lit_advanced";// "lit"; //"textured"; //"normal";  //"textured"; //"default";

        int ibo_elements;

        Dictionary<String, Material> materials = new Dictionary<string, Material>();

        Vector3[] vertdata;
        Vector3[] coldata;
        int[] indicedata;

        List<Volume> objects = new List<Volume>();

        float time = 0.0f;

        Light activeLight = new Light(new Vector3(), new Vector3(0.9f, 0.80f, 0.8f));

        bool updateFirstThenRender = false;

        Camera cam = new Camera();
        Vector2 lastMousePos = new Vector2();

        Dictionary<string, int> textures = new Dictionary<string, int>();
        Vector2[] texcoorddata;
        Vector3[] normdata;

        Dictionary<string, ObjVolume> loadedObjFiles = new Dictionary<string, ObjVolume>();
        
        Matrix4 view = Matrix4.Identity;

        List<Light> lights = new List<Light>();
        const int MAX_LIGHTS = 5;

        InputCntl InputCntl = new InputCntl();
        ObjVolume SpherObjVol;

        public ParticleAddedEvent NewParticleAdded { get; }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            initProgram();

            Title = "GOD AI";

            GL.ClearColor(new OpenTK.Color(30, 30, 30, 255));
           // GL.ClearColor(Color.DarkBlue);

            GL.PointSize(5f);
 
        }


        void loadResources()
        {
            // Load shaders from file
            shaders.Add("default", new ShaderProgram("Shaders/vs.glsl", "Shaders/fs.glsl", true));
            shaders.Add("textured", new ShaderProgram("Shaders/vs_tex.glsl", "Shaders/fs_tex.glsl", true));
            shaders.Add("normal", new ShaderProgram("Shaders/vs_norm.glsl", "Shaders/fs_norm.glsl", true));
            shaders.Add("lit", new ShaderProgram("Shaders/vs_lit.glsl", "Shaders/fs_lit.glsl", true));
            shaders.Add("lit_multiple", new ShaderProgram("Shaders/vs_lit.glsl", "Shaders/fs_lit_multiple.glsl", true));
            shaders.Add("lit_advanced", new ShaderProgram("Shaders/vs_lit.glsl", "Shaders/fs_lit_advanced.glsl", true));
            shaders.Add("colorAdvanced", new ShaderProgram("Shaders/vs_colorAdvanced.glsl", "Shaders/fs_colorAdvanced.glsl", true));

            loadMaterials("Materials/opentk.mtl");
            loadMaterials("Materials/earth.mtl");

            SpherObjVol = new ObjVolume();
            ObjVolume.LoadFromFile("3DModels/sphere.obj", SpherObjVol);
            loadedObjFiles.Add("SphereObj", SpherObjVol);

        }

        private void setupScene()
        {
            //// Create our objects
            //TexturedCube tc = new TexturedCube();
            //tc.TextureID = textures[materials["opentk1"].DiffuseMap];
            //tc.CalculateNormals();
            //tc.Material = materials["opentk1"];
            //objects.Add(tc);

            //TexturedCube tc2 = new TexturedCube();
            //tc2.Position += new Vector3(1f, 1f, 1f);
            //tc2.TextureID = textures[materials["opentk2"].DiffuseMap];
            //tc2.CalculateNormals();
            //tc2.Material = materials["opentk2"];
            //objects.Add(tc2);

       
            //TexturedCube floor = new TexturedCube();
            //floor.TextureID = textures[materials["opentk1"].DiffuseMap];
            //floor.Scale = new Vector3(20, 0.1f, 20);
            //floor.Position += new Vector3(0, -2, 0);
            //floor.CalculateNormals();
            //floor.Material = materials["opentk1"];
            //objects.Add(floor);

            //TexturedCube backWall = new TexturedCube();
            //backWall.TextureID = textures[materials["opentk1"].DiffuseMap];
            //backWall.Scale = new Vector3(20, 20, 0.1f);
            //backWall.Position += new Vector3(0, 8, -10);
            //backWall.CalculateNormals();
            //backWall.Material = materials["opentk1"];
            //objects.Add(backWall);

            //ObjVolume earth = ObjVolume.LoadFromFile("3DModels/sphere.obj");
            //earth.TextureID = textures["earth.png"];
            //earth.Position += new Vector3(1f, 1f, -2f);
            //earth.Material = materials["earth"]; //new Material(new Vector3(0.15f), new Vector3(1), new Vector3(0.2f), 5);
            //objects.Add(earth);


          //  foreach (var v in _universe.Particles)
           // {
              //  v.TextureID = textures["earth.png"];
               // v.Material = materials["earth"];
                //v.LoadFromAnotherObjVolume(SpherObjVol);
           // }
            objects.AddRange(_universe.Particles);

            // Create lights
            Light sunLight = new Light(new Vector3(), new Vector3(0.3f, 0.3f, 0.3f));
            sunLight.Type = LightType.Directional;
            sunLight.Direction = (sunLight.Position).Normalized();
            lights.Add(sunLight);

            Light pointLight = new Light(new Vector3(2, 7, 0), new Vector3(1.5f, 0.2f, 0.2f), 0.02f, 0.02f);
            pointLight.QuadraticAttenuation = 0.05f;
            lights.Add(pointLight);

            Light pointLight2 = new Light(new Vector3(2, 0, 3), new Vector3(0.2f, 1f, 0.25f), 0.02f, 0.02f);
            pointLight2.QuadraticAttenuation = 0.05f;
            lights.Add(pointLight2);

            Light pointLight3 = new Light(new Vector3(6, 4, 0), new Vector3(0.2f, 0.25f, 1.5f), 0.02f, 0.02f);
            pointLight3.QuadraticAttenuation = 0.05f;
            lights.Add(pointLight3);

            // Move camera away from origin
            cam.Position += new Vector3(0f, 0f, 300f);
        }

        void initProgram()
        {
            //objects.Add(new Cube());
            //objects.Add(new Cube());

            GL.GenBuffers(1, out ibo_elements);

            loadResources();
 
            lastMousePos = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
            CursorVisible = true;

            setupScene();

        }

       
        void loadShader(String filename, ShaderType type, int program, out int address)
        {
            address = GL.CreateShader(type);
            using (StreamReader sr = new StreamReader(filename))
            {
                GL.ShaderSource(address, sr.ReadToEnd());
            }
            GL.CompileShader(address);
            GL.AttachShader(program, address);
            Console.WriteLine(GL.GetShaderInfoLog(address));
        }
 

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            InputCntl.ProcessInput(cam, Focused, ref lastMousePos);

            List<Vector3> verts = new List<Vector3>();
            List<int> inds = new List<int>();
            List<Vector3> colors = new List<Vector3>();
            List<Vector2> texcoords = new List<Vector2>();
            List<Vector3> normals = new List<Vector3>();

            int vertcount = 0;
            lock (objects)
            {
                foreach (Volume v in objects)
                {
                    verts.AddRange(v.GetVerts().ToList());
                    inds.AddRange(v.GetIndices(vertcount).ToList());
                    colors.AddRange(v.GetColorData().ToList());
                    texcoords.AddRange(v.GetTextureCoords());
                    normals.AddRange(v.GetNormals().ToList());
                    vertcount += v.VertCount;
                }
            }
 
            vertdata = verts.ToArray();
            indicedata = inds.ToArray();
            coldata = colors.ToArray();
            texcoorddata = texcoords.ToArray();
            normdata = normals.ToArray();

            GL.BindBuffer(BufferTarget.ArrayBuffer, shaders[activeShader].GetBuffer("vPosition"));

            GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, (IntPtr)(vertdata.Length * Vector3.SizeInBytes), vertdata, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(shaders[activeShader].GetAttribute("vPosition"), 3, VertexAttribPointerType.Float, false, 0, 0);

            // Buffer vertex color if shader supports it
            if (shaders[activeShader].GetAttribute("vColor") != -1)
            {
                GL.BindBuffer(BufferTarget.ArrayBuffer, shaders[activeShader].GetBuffer("vColor"));
                GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, (IntPtr)(coldata.Length * Vector3.SizeInBytes), coldata, BufferUsageHint.StaticDraw);
                GL.VertexAttribPointer(shaders[activeShader].GetAttribute("vColor"), 3, VertexAttribPointerType.Float, true, 0, 0);
            }


            // Buffer texture coordinates if shader supports it
            if (shaders[activeShader].GetAttribute("texcoord") != -1)
            {
                GL.BindBuffer(BufferTarget.ArrayBuffer, shaders[activeShader].GetBuffer("texcoord"));
                GL.BufferData<Vector2>(BufferTarget.ArrayBuffer, (IntPtr)(texcoorddata.Length * Vector2.SizeInBytes), texcoorddata, BufferUsageHint.StaticDraw);
                GL.VertexAttribPointer(shaders[activeShader].GetAttribute("texcoord"), 2, VertexAttribPointerType.Float, true, 0, 0);
            }

            if (shaders[activeShader].GetAttribute("vNormal") != -1)
            {
                GL.BindBuffer(BufferTarget.ArrayBuffer, shaders[activeShader].GetBuffer("vNormal"));
                GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, (IntPtr)(normdata.Length * Vector3.SizeInBytes), normdata, BufferUsageHint.StaticDraw);
                GL.VertexAttribPointer(shaders[activeShader].GetAttribute("vNormal"), 3, VertexAttribPointerType.Float, true, 0, 0);
            }

            //time += (float)e.Time;

            //objects[0].Position = new Vector3(0.3f, -0.5f + (float)Math.Sin(time), -3.0f);
            //objects[0].Rotation = new Vector3(0.55f * time, 0.25f * time, 0);
            //objects[0].Scale = new Vector3(0.5f, 0.5f, 0.5f);

            //objects[1].Position = new Vector3(-1f, 0.5f + (float)Math.Cos(time), -2.0f);
            //objects[1].Rotation = new Vector3(-0.25f * time, -0.35f * time, 0);
            //objects[1].Scale = new Vector3(0.7f, 0.7f, 0.7f);

            //lock (_universe.Particles)
            //{
            //    foreach (Volume v in _universe.Particles)
            //    {
            //        v.CalculateModelMatrix();
            //        // v.ViewProjectionMatrix = Matrix4.CreatePerspectiveFieldOfView(1.3f, ClientSize.Width / (float)ClientSize.Height, 1.0f, 40.0f);
            //        v.ViewProjectionMatrix = cam.GetViewMatrix() * Matrix4.CreatePerspectiveFieldOfView(1.3f, ClientSize.Width / (float)ClientSize.Height, 1.0f, 40000.0f);
            //        v.ModelViewProjectionMatrix = v.ModelMatrix * v.ViewProjectionMatrix;
            //    }
            //}

            // Update model view matrices
            lock (objects)
            {
                foreach (Volume v in objects)
                {
                    double scaleFactor = (v as Gerticle).GerFactor/10;
                    v.Scale = new Vector3((float)scaleFactor, (float)scaleFactor, (float)scaleFactor);
                    v.CalculateModelMatrix();
                    v.ViewProjectionMatrix = cam.GetViewMatrix() * Matrix4.CreatePerspectiveFieldOfView(1.3f, ClientSize.Width / (float)ClientSize.Height, 1.0f, 100000.0f);
                    v.ModelViewProjectionMatrix = v.ModelMatrix * v.ViewProjectionMatrix;
                }
            }
 
            GL.UseProgram(shaders[activeShader].ProgramID);

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ibo_elements);
            GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(indicedata.Length * sizeof(int)), indicedata, BufferUsageHint.StaticDraw);

            view = cam.GetViewMatrix();

            if (!updateFirstThenRender) updateFirstThenRender = true;
        }

       

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            if(!updateFirstThenRender) return;
            GL.Viewport(0, 0, Width, Height);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.Enable(EnableCap.DepthTest);

            GL.UseProgram(shaders[activeShader].ProgramID);
            shaders[activeShader].EnableVertexAttribArrays();

            int indiceat = 0;
            lock (objects)
            {
                // Draw all objects
                foreach (Volume v in objects)
                {
                    GL.BindTexture(TextureTarget.Texture2D, v.TextureID);

                    GL.UniformMatrix4(shaders[activeShader].GetUniform("modelview"), false, ref v.ModelViewProjectionMatrix);

                    if (shaders[activeShader].GetAttribute("maintexture") != -1)
                    {
                        GL.Uniform1(shaders[activeShader].GetAttribute("maintexture"), v.TextureID);
                    }

                    if (shaders[activeShader].GetUniform("view") != -1)
                    {
                        GL.UniformMatrix4(shaders[activeShader].GetUniform("view"), false, ref view);
                    }

                    if (shaders[activeShader].GetUniform("model") != -1)
                    {
                        GL.UniformMatrix4(shaders[activeShader].GetUniform("model"), false, ref v.ModelMatrix);
                    }

                    if (shaders[activeShader].GetUniform("material_ambient") != -1)
                    {
                        GL.Uniform3(shaders[activeShader].GetUniform("material_ambient"), ref v.Material.AmbientColor);
                    }

                    if (shaders[activeShader].GetUniform("material_diffuse") != -1)
                    {
                        GL.Uniform3(shaders[activeShader].GetUniform("material_diffuse"), ref v.Material.DiffuseColor);
                    }

                    if (shaders[activeShader].GetUniform("material_specular") != -1)
                    {
                        GL.Uniform3(shaders[activeShader].GetUniform("material_specular"), ref v.Material.SpecularColor);
                    }

                    if (shaders[activeShader].GetUniform("material_specExponent") != -1)
                    {
                        GL.Uniform1(shaders[activeShader].GetUniform("material_specExponent"), v.Material.SpecularExponent);
                    }

                    if (shaders[activeShader].GetUniform("map_specular") != -1)
                    {
                        // Object has a specular map
                        if (v.Material.SpecularMap != "")
                        {
                            GL.ActiveTexture(TextureUnit.Texture1);
                            GL.BindTexture(TextureTarget.Texture2D, textures[v.Material.SpecularMap]);
                            GL.Uniform1(shaders[activeShader].GetUniform("map_specular"), 1);
                            GL.Uniform1(shaders[activeShader].GetUniform("hasSpecularMap"), 1);
                            GL.ActiveTexture(TextureUnit.Texture0);
                        }
                        else // Object has no specular map
                        {
                            GL.Uniform1(shaders[activeShader].GetUniform("hasSpecularMap"), 0);
                        }
                    }

                    if (shaders[activeShader].GetUniform("light_position") != -1)
                    {
                        GL.Uniform3(shaders[activeShader].GetUniform("light_position"), ref lights[0].Position);
                    }

                    if (shaders[activeShader].GetUniform("light_color") != -1)
                    {
                        GL.Uniform3(shaders[activeShader].GetUniform("light_color"), ref lights[0].Color);
                    }

                    if (shaders[activeShader].GetUniform("light_diffuseIntensity") != -1)
                    {
                        GL.Uniform1(shaders[activeShader].GetUniform("light_diffuseIntensity"), lights[0].DiffuseIntensity);
                    }

                    if (shaders[activeShader].GetUniform("light_ambientIntensity") != -1)
                    {
                        GL.Uniform1(shaders[activeShader].GetUniform("light_ambientIntensity"), lights[0].AmbientIntensity);
                    }


                    for (int i = 0; i < Math.Min(lights.Count, MAX_LIGHTS); i++)
                    {
                        if (shaders[activeShader].GetUniform("lights[" + i + "].position") != -1)
                        {
                            GL.Uniform3(shaders[activeShader].GetUniform("lights[" + i + "].position"), ref lights[i].Position);
                        }

                        if (shaders[activeShader].GetUniform("lights[" + i + "].color") != -1)
                        {
                            GL.Uniform3(shaders[activeShader].GetUniform("lights[" + i + "].color"), ref lights[i].Color);
                        }

                        if (shaders[activeShader].GetUniform("lights[" + i + "].diffuseIntensity") != -1)
                        {
                            GL.Uniform1(shaders[activeShader].GetUniform("lights[" + i + "].diffuseIntensity"), lights[i].DiffuseIntensity);
                        }

                        if (shaders[activeShader].GetUniform("lights[" + i + "].ambientIntensity") != -1)
                        {
                            GL.Uniform1(shaders[activeShader].GetUniform("lights[" + i + "].ambientIntensity"), lights[i].AmbientIntensity);
                        }

                        if (shaders[activeShader].GetUniform("lights[" + i + "].direction") != -1)
                        {
                            GL.Uniform3(shaders[activeShader].GetUniform("lights[" + i + "].direction"), ref lights[i].Direction);
                        }

                        if (shaders[activeShader].GetUniform("lights[" + i + "].type") != -1)
                        {
                            GL.Uniform1(shaders[activeShader].GetUniform("lights[" + i + "].type"), (int)lights[i].Type);
                        }

                        if (shaders[activeShader].GetUniform("lights[" + i + "].coneAngle") != -1)
                        {
                            GL.Uniform1(shaders[activeShader].GetUniform("lights[" + i + "].coneAngle"), lights[i].ConeAngle);
                        }

                        if (shaders[activeShader].GetUniform("lights[" + i + "].linearAttenuation") != -1)
                        {
                            GL.Uniform1(shaders[activeShader].GetUniform("lights[" + i + "].linearAttenuation"), lights[i].LinearAttenuation);
                        }

                        if (shaders[activeShader].GetUniform("lights[" + i + "].quadraticAttenuation") != -1)
                        {
                            GL.Uniform1(shaders[activeShader].GetUniform("lights[" + i + "].quadraticAttenuation"), lights[i].QuadraticAttenuation);
                        }
                    }

                    GL.DrawElements(BeginMode.Triangles, v.IndiceCount, DrawElementsType.UnsignedInt, indiceat * sizeof(uint));
                    indiceat += v.IndiceCount;
                }

            }
            shaders[activeShader].DisableVertexAttribArrays();

            GL.Flush();
            SwapBuffers();
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            if(e.Mouse.LeftButton == ButtonState.Pressed)
            {
                InputCntl.MouseDwn = true;
                lastMousePos = new Vector2(OpenTK.Input.Mouse.GetState().X, OpenTK.Input.Mouse.GetState().Y);
            }
        }


        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);

            if(e.Mouse.LeftButton == ButtonState.Released)
            {
                InputCntl.MouseDwn = false;
            }
        }


        protected override void OnKeyUp(KeyboardKeyEventArgs e)
        {
            base.OnKeyUp(e);
            if (e.Key == Key.W)
            {
                InputCntl.WkeyDwn = false;
            }

            if (e.Key == Key.S)
            {
                InputCntl.SkeyDwn = false;
            }

            if (e.Key == Key.A)
            {
                InputCntl.AkeyDwn = false;
            }

            if (e.Key == Key.D)
            {
                InputCntl.DkeyDwn = false;
            }

            if (e.Key == Key.Q)
            {
                InputCntl.QkeyDwn = false;
            }

            if (e.Key == Key.E)
            {
                InputCntl.EkeyDwn = false;
            }

            if (e.Key == Key.Left)
            {
                InputCntl.LeftkeyDwn = false;
            }

            if (e.Key == Key.Right)
            {
                InputCntl.RightkeyDwn = false;
            }


            if (e.Key == Key.Up)
            {
                InputCntl.UpkeyDwn = false;
            }

            if (e.Key == Key.Down)
            {
                InputCntl.DownkeyDwn = false;
            }
        }
   
        protected override void OnKeyDown(KeyboardKeyEventArgs e)
        {
            base.OnKeyDown(e);

            if (e.Key == Key.W)
            {
                InputCntl.WkeyDwn = true;
            }

            if (e.Key == Key.S)
            {
                InputCntl.SkeyDwn = true;
            }

            if (e.Key == Key.A)
            {
                InputCntl.AkeyDwn = true;
            }

            if (e.Key == Key.D)
            {
                InputCntl.DkeyDwn = true;
            }

            if (e.Key == Key.Q)
            {
                InputCntl.QkeyDwn = true;
            }

            if (e.Key == Key.E)
            {
                InputCntl.EkeyDwn = true;
            }

            if (e.Key == Key.Left)
            {
                InputCntl.LeftkeyDwn = true;
            }

            if (e.Key == Key.Right)
            {
                InputCntl.RightkeyDwn = true;
            }

            if (e.Key == Key.Up)
            {
                InputCntl.UpkeyDwn = true;
            }

            if (e.Key == Key.Down)
            {
                InputCntl.DownkeyDwn = true;
            }

        }
       

        
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            GL.Viewport(ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width, ClientRectangle.Height);
            
            Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView((float)Math.PI /4, Width / (float)Height, 1.0f, 64.0f);
            GL.MatrixMode(MatrixMode.Projection);

            GL.LoadMatrix(ref projection);
        }

        protected override void OnFocusedChanged(EventArgs e)
        {
            base.OnFocusedChanged(e);
            lastMousePos = new Vector2(OpenTK.Input.Mouse.GetState().X, OpenTK.Input.Mouse.GetState().Y);
        }


        int loadImage(System.Drawing.Bitmap image) /// Load texture image
        {
            int texID = GL.GenTexture();

            GL.BindTexture(TextureTarget.Texture2D, texID);
            BitmapData data = image.LockBits(new System.Drawing.Rectangle(0, 0, image.Width, image.Height),
                ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0,
                OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);

            image.UnlockBits(data);

            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);

            return texID;
        }

        int loadImage(string filename) // Load image for texture overload
        {
            try
            {
                System.Drawing.Bitmap file = new System.Drawing.Bitmap(filename);
                return loadImage(file);
            }
            catch (FileNotFoundException e)
            {
                return -1;
            }
        }


        private void loadMaterials(String filename)
        {
            foreach (var mat in Material.LoadFromFile(filename))
            {
                if (!materials.ContainsKey(mat.Key))
                {
                    materials.Add(mat.Key, mat.Value);
                }
            }

            // Load textures
            foreach (Material mat in materials.Values)
            {
                if (File.Exists("Textures/" + mat.AmbientMap) && !textures.ContainsKey(mat.AmbientMap))
                {
                    textures.Add(mat.AmbientMap, loadImage("Textures/" + mat.AmbientMap));
                }

                if (File.Exists("Textures/" + mat.DiffuseMap) && !textures.ContainsKey(mat.DiffuseMap))
                {
                    textures.Add(mat.DiffuseMap, loadImage("Textures/" + mat.DiffuseMap));
                }

                if (File.Exists("Textures/" + mat.SpecularMap) && !textures.ContainsKey(mat.SpecularMap))
                {
                    textures.Add(mat.SpecularMap, loadImage("Textures/" + mat.SpecularMap));
                }

                if (File.Exists("Textures/" + mat.NormalMap) && !textures.ContainsKey(mat.NormalMap))
                {
                    textures.Add(mat.NormalMap, loadImage("Textures/" + mat.NormalMap));
                }

                if (File.Exists("Textures/" + mat.OpacityMap) && !textures.ContainsKey(mat.OpacityMap))
                {
                    textures.Add(mat.OpacityMap, loadImage("Textures/" + mat.OpacityMap));
                }
            }
        }
    }
}