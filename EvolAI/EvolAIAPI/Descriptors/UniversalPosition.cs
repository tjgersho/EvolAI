using System;
using System.Collections.Generic;
using System.Text;
using GodAIAPI.Utils;
namespace GodAIAPI.Descriptors
{
    public class UniversalPosition
    {
        public double X = 1000 * GodAIUtils.rand.NextDouble() - 500;
        public double Y = 1000 * GodAIUtils.rand.NextDouble() - 500;
        public double Z = 1000 * GodAIUtils.rand.NextDouble() - 500;

        public UniversalPosition()
        {
            while(Math.Sqrt(X*X + Y*Y + Z*Z) > 500)
            {
                X = 1000 * GodAIUtils.rand.NextDouble() - 500;
                Y = 1000 * GodAIUtils.rand.NextDouble() - 500;
                Z = 1000 * GodAIUtils.rand.NextDouble() - 500;
            }

        }
       

        public double t = 0;
    }
}
