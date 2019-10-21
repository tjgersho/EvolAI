using System;
using System.Collections.Generic;
using System.Text;

namespace GodAIAPI.Utils
{
    public static class GodAIUtils
    {

        public static double GravitationConst => 0.001;

        public static Random rand = new Random();
        public static int GetRandomBetween(int min, int max)
        {
            return (int)(rand.NextDouble() * (max - min) + min);
        }
    }
}
