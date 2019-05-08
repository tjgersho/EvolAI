using System;
using System.Collections.Generic;
using System.Text;

namespace GodAIAPI
{
    public static class Utils
    {
        public static Random rand = new Random();
        public static int GetRandomBetween(int min, int max)
        {
            return (int)(rand.NextDouble() * (max - min) + min);
        }
    }
}
