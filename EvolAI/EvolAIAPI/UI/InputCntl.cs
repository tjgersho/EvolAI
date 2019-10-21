using OpenTK;

namespace GodAIAPI.UI
{
    public class InputCntl
    {
        public bool QkeyDwn { get; set; } = false;
        public bool WkeyDwn { get; set; } = false;
        public bool EkeyDwn { get; set; } = false;
        public bool AkeyDwn { get; set; } = false;
        public bool SkeyDwn { get; set; } = false;
        public bool DkeyDwn { get; set; } = false;

        public bool LeftkeyDwn { get; set; } = false;
        public bool RightkeyDwn { get; set; } = false;
        public bool UpkeyDwn { get; set; } = false;
        public bool DownkeyDwn { get; set; } = false;

        public bool MouseDwn { get; set; } = false;

        public void ProcessInput(Camera cam, bool focused, ref Vector2 lastMousePos)
        {
            if (WkeyDwn)
            {
                cam.Move(0f, 0.1f, 0f);
            }

            if (SkeyDwn)
            {
                cam.Move(0f, -0.1f, 0f);
            }

            if (AkeyDwn)
            {
                cam.Move(-0.1f, 0f, 0f);
            }

            if (DkeyDwn)
            {
                cam.Move(0.1f, 0f, 0f);
            }

            if (QkeyDwn)
            {
                cam.Move(0f, 0f, 0.1f);
            }

            if (EkeyDwn) // Fix
            {
                cam.Move(0f, 0f, -0.1f);
            }

            if (LeftkeyDwn)
            {
                cam.AddRotation(10, 0);
            }

            if (RightkeyDwn)
            {
                cam.AddRotation(-10, 0);
            }


            if (UpkeyDwn)
            {
                cam.AddRotation(0, 10);
            }

            if (DownkeyDwn)
            {
                cam.AddRotation(0, -10);
            }

            if (focused && MouseDwn)
            {
                Vector2 delta = lastMousePos - new Vector2(OpenTK.Input.Mouse.GetState().X, OpenTK.Input.Mouse.GetState().Y);
                lastMousePos += delta;

                cam.AddRotation(delta.X, delta.Y);
                lastMousePos = new Vector2(OpenTK.Input.Mouse.GetState().X, OpenTK.Input.Mouse.GetState().Y);
            }
        }
    }
}