using System;
using System.Collections.Generic;
using System.Text;

namespace GodAIAPI.Descriptors
{
   public class UniversalAcceleration
    {
        public double X = 2 * Utils.rand.NextDouble() - 1;
        public double Y = 2 * Utils.rand.NextDouble() - 1;
        public double Z = 2 * Utils.rand.NextDouble() - 1;
    }
}
