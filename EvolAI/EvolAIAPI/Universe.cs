using System;
using System.Collections.Generic;
using System.Text;

using GodAIAPI.BuildingBlocks;
using GodAIAPI.Descriptors;
using GodAIAPI.Utils;

namespace GodAIAPI
{
    public class Universe
    {
        public List<Particle> Particles { get; set; } = new List<Particle>();
        double timeToAddParticle = 0;
        public event EventHandler<ParticleAddedEvent> ParticleAdded;
        public TreeSearch3D TS3D = new TreeSearch3D();

        public Universe()
        {
            while (Particles.Count < 1000)
            {
                AddParticle();
            }

        }
        public void Update(double dt)
        {
            //Console.WriteLine(Particles[0].UPos.X);
            lock (Particles)
            {
                foreach (Gerticle part in Particles)
                {

                    double fx = 0;
                    double fy = 0;
                    double fz = 0;

                    var nearByParticles = TS3D.GetNearbyParticles(part);
                    nearByParticles = Particles;  // Hack.. remove when TS3D is working..

                    foreach (Gerticle ticle in nearByParticles)
                    {
                        if (ticle.Guid != part.Guid)
                        {
                            double xdist = (ticle.Position.X - part.Position.X);
                            if (Math.Abs(xdist) > 10)
                            {
                                fx += GodAIUtils.GravitationConst * (ticle.GerFactor * part.GerFactor) / (xdist);
                            }
                            double ydist = (ticle.Position.Y - part.Position.Y);
                            if (Math.Abs(ydist) > 10)
                            {
                                fy += GodAIUtils.GravitationConst * (ticle.GerFactor * part.GerFactor) / (ydist);
                            }
                            double zdist = (ticle.Position.Z - part.Position.Z);
                            if (Math.Abs(zdist) > 10)
                            {
                                fz += GodAIUtils.GravitationConst * (ticle.GerFactor * part.GerFactor) / (zdist);
                            }
                        }
                    }

                    double ax = fx / part.GerFactor;
                    double ay = fy / part.GerFactor;
                    double az = fz / part.GerFactor;

 
                    part.UAccel.X += ax;
                    part.UAccel.Y += ay;
                    part.UAccel.Z += az;

                    /// damping
                    part.UVel.X /= 1.1;
                    part.UVel.Y /= 1.1;
                    part.UVel.Z /= 1.1;

                    var newUnivPos = new UniversalPosition();
                    newUnivPos.X = 0.5 * part.UAccel.X * Math.Pow(dt, 2) + part.UVel.X * dt + part.GetUPos().X;
                    newUnivPos.Y = 0.5 * part.UAccel.Y * Math.Pow(dt, 2) + part.UVel.Y * dt + part.GetUPos().Y;
                    newUnivPos.Z = 0.5 * part.UAccel.Z * Math.Pow(dt, 2) + part.UVel.Z * dt + part.GetUPos().Z;
                    part.SetUPos(newUnivPos);
                }
            }


            timeToAddParticle += dt;
            if (timeToAddParticle > 0.1)
            {
                if (Particles.Count <= 1000)
                {
                    AddParticle();
                }
                if(Particles.Count == 499)
                {
                    Console.WriteLine("500 Particles");
                }
                timeToAddParticle = 0;
            //    Console.WriteLine("Particle Count " + Particles.Count);
            }

            //Calculate collisions..
         

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
