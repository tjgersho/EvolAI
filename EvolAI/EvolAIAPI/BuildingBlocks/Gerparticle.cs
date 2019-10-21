using System;
using System.Collections.Generic;
using System.Text;
using GodAIAPI.UI;
using GodAIAPI.Utils;

namespace GodAIAPI.BuildingBlocks
{
    public class Gerticle : Particle 
    {
        Random rand = new Random(DateTime.Now.Millisecond);
        public Gerticle():base()
        {
            GerIndex = GodAIUtils.GetRandomBetween(1, 10);
            GerFactor = GodAIUtils.GetRandomBetween(1, 100);
           
            
          SetColor(new OpenTK.Vector3((float)rand.NextDouble(), (float)rand.NextDouble(), (float)rand.NextDouble()));
        }

        public int GerIndex { get; set; }

        public int GerFactor { get; set; }

    }
}
