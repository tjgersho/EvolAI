using GodAIAPI.BuildingBlocks;
using System;

namespace GodAIAPI
{
    public class ParticleAddedEvent : EventArgs
    {
        public ParticleAddedEvent(Particle addedParticle)
        {
            NewParticle = addedParticle;
        }

        public Particle NewParticle { get; }
    }
}