using System;
using System.Collections.Generic;
using System.Text;

namespace GodAIAPI.BuildingBlocks
{
    public class Gerticle : Particle
    {
        public Gerticle()
        {
            GerIndex = Utils.GetRandomBetween(1, 10);
            GerCharge = Utils.GetRandomBetween(0, 2);
            while (GerIndex - GerCharge <= 0) {
                GerCharge = Utils.GetRandomBetween(0, 2);
            }

        }
        public int GerIndex { get; set; }

        public int GerCharge { get; set; }

    }
}
