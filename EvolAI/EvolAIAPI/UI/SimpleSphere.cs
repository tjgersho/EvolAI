using System;
using System.Collections.Generic;
using System.Text;
using OpenTK;

namespace GodAIAPI.UI
{
    public class SimpleSphere : Volume
    {

        protected Vector3 Color = new Vector3(1, 1, 1);
 
        public SimpleSphere()
        {
            VertCount = 24;
            IndiceCount = 132;
            ColorDataCount = 24;
            IsTextured = false;
        }

        public override void CalculateModelMatrix()
        {
            ModelMatrix = Matrix4.CreateScale(Scale) *
                            Matrix4.CreateRotationX(Rotation.X) *
                                Matrix4.CreateRotationY(Rotation.Y) *
                                    Matrix4.CreateRotationZ(Rotation.Z) *
                                            Matrix4.CreateTranslation(Position);

        }

        public override Vector3[] GetColorData()
        {
            return new Vector3[] {
            Color,
            Color,
            Color,
            Color,
            Color,
            Color,
            Color,
            Color,
             Color,
            Color,
            Color,
            Color,
            Color,
            Color,
            Color,
            Color,
             Color,
            Color,
            Color,
            Color,
            Color,
            Color,
            Color,
            Color
                //new Vector3( 1f, 0f, 0f),
                //new Vector3( 0f, 0f, 1f),
                //new Vector3( 0f, 1f, 0f),
                //new Vector3( 1f, 0f, 0f),
                //new Vector3( 0f, 0f, 1f),
                //new Vector3( 0f, 1f, 0f),
                //new Vector3( 1f, 0f, 0f),
                //new Vector3( 0f, 0f, 1f),
                //new Vector3( 1f, 0f, 0f),
                //new Vector3( 0f, 0f, 1f),
                //new Vector3( 0f, 1f, 0f),
                //new Vector3( 1f, 0f, 0f),
                //new Vector3( 0f, 0f, 1f),
                //new Vector3( 0f, 1f, 0f),
                //new Vector3( 1f, 0f, 0f),
                //new Vector3( 0f, 0f, 1f),
                //new Vector3( 1f, 0f, 0f),
                //new Vector3( 0f, 0f, 1f),
                //new Vector3( 0f, 1f, 0f),
                //new Vector3( 1f, 0f, 0f),
                //new Vector3( 0f, 0f, 1f),
                //new Vector3( 0f, 1f, 0f),
                //new Vector3( 1f, 0f, 0f),
                //new Vector3( 0f, 0f, 1f)
               };
        }

        public override int[] GetIndices(int offset = 0)
        {  //Connect the dots!
            int[] inds = new int[] {
               20,21,16,
               16,21,17,
               17,21,22,
               17,22,18,
               18,22,23,
               18,23,19,

               0,16,17,
               0,17,1,
               1,17,18,
               1,18,2,
               2,18,19,
               2,19,3,

               0,1,8,
               8,1,9,
               1,2,9,
               9,2,10,
               2,3,10,
               10,3,11,

               8,9,12,
               12,9,13,
               9,10,13,
               13,10,14,
               14,10,11,
               14,11,15,

               12,13,4,
               4,13,5,
               5,13,14,
               5,14,6,
               6,14,15,
               6,15,7,


               4,5,20,
               20,5,21,
               21,5,6,
               21,6,22,
               22,6,7,
               22,7,23,

               4,20,12,
               12,20,16,
               12,16,8,
               8,16,0,

               7,23,15,
               15,23,19,
               15,19,11,
               11,19,3







            };

            if (offset != 0)
            {
                for (int i = 0; i < inds.Length; i++)
                {
                    inds[i] += offset;
                }
            }

            return inds;
        }

        public override Vector3[] GetVerts()
        {
            
        return new Vector3[] {
            new Vector3(0.34f,  0.94f, 0.00f), //    1
            new Vector3(0.87f,  0.50f, 0.00f), //    2
            new Vector3(0.87f, -0.50f, 0.00f), //    3
            new Vector3(0.34f, -0.94f, 0.00f), //    4

            new Vector3(-0.34f,  0.94f, 0.00f),//   5
            new Vector3(-0.87f,  0.50f, 0.00f),//    6
            new Vector3(-0.87f, -0.50f, 0.00f),//   7
            new Vector3(-0.34f, -0.94f, 0.00f),//   8



            new Vector3(0.17f, 0.94f, 0.30f),//   9
            new Vector3(0.43f, 0.50f, 0.75f),//    10
            new Vector3(0.43f, -0.50f, 0.75f),//   11
            new Vector3(0.17f, -0.94f, 0.30f),//   12

            new Vector3(-0.17f, 0.94f, 0.30f),//    13
            new Vector3(-0.43f, 0.50f, 0.75f),//   14
            new Vector3(-0.43f, -0.50f, 0.75f),//   15
            new Vector3(-0.17f, -0.94f, 0.30f),//    16


            new Vector3(0.17f, 0.94f, -0.30f),//  17
            new Vector3(0.43f, 0.50f, -0.75f),//  18
            new Vector3(0.43f, -0.50f, -0.75f),//  19
            new Vector3(0.17f, -0.94f, -0.30f),//  20

            new Vector3(-0.17f, 0.94f, -0.30f),//  21
            new Vector3(-0.43f, 0.50f, -0.75f),//  22
            new Vector3(-0.43f, -0.50f, -0.75f),//  23
            new Vector3(-0.17f, -0.94f, -0.30f)//  24
        };
        }
        public void SetColor(Vector3 color)
        {
            Color = color;
        }
        public override Vector2[] GetTextureCoords()
        {
            return new Vector2[] { };
        }
    }
}
