using System;
using System.Collections.Generic;
using System.Text;

using GodAIAPI.BuildingBlocks;
using GodAIAPI.Descriptors;

namespace GodAIAPI
{
    public class Universe
    {
        public List<Particle> Particles { get; set; } = new List<Particle>();
        double timeToAddParticle = 0;
        public event EventHandler<ParticleAddedEvent> ParticleAdded;
        public void Update(double dt)
        {
            //Console.WriteLine(Particles[0].UPos.X);
   
                foreach (var part in Particles)
                {
                    var newUnivPos = new UniversalPosition();
                    newUnivPos.X = 0.5 * part.UAccel.X * Math.Pow(dt, 2) + part.UVel.X * dt + part.GetUPos().X;
                    newUnivPos.Y = 0.5 * part.UAccel.Y * Math.Pow(dt, 2) + part.UVel.Y * dt + part.GetUPos().Y;
                    newUnivPos.Z = 0.5 * part.UAccel.Z * Math.Pow(dt, 2) + part.UVel.Z * dt + part.GetUPos().Z;
                    part.SetUPos(newUnivPos);

                }


            timeToAddParticle += dt;
            if (timeToAddParticle > 1)
            {
                AddParticle();
                timeToAddParticle = 0;
            }

            ////Calculate collisions..
            ///

            //Calculate merge compatibility..

            //Cacluate relative split vector and determine if split threashold is met.

        }

        void AddParticle()
        {
            lock (Particles)
            {
                var particle = new Gerticle();
                particle.Position.X = (float)particle.GetUPos().X;
                particle.Position.Y = (float)particle.GetUPos().Y;
                particle.Position.Z = (float)particle.GetUPos().Z;
                particle.CalculateNormals();
                Particles.Add(particle);

                ParticleAdded?.Invoke(this, new ParticleAddedEvent(particle));
                                
            }
        }

    }
}
