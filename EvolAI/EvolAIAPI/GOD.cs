using Newtonsoft.Json;
using System;
using System.IO;
using GodAIAPI.BuildingBlocks;
using GodAIAPI.UI;
using System.Threading.Tasks;

namespace GodAIAPI
{
    public static class GOD
    {
        private static double Score { get; set; }

        private static UniverseSeed universeSeed { get; set; }

        public static Universe Universe { get; set; } = new Universe();

        private static string seedFileName {get; set;}

        public static void SeedUniverse(string fileName = "seed")
        {
            seedFileName = fileName;
            // deserialize JSON directly from a file
            StreamReader file = File.OpenText(@"SeedFile/" + fileName + ".json");
        
            JsonSerializer serializer = new JsonSerializer();
            universeSeed = (UniverseSeed)serializer.Deserialize(file, typeof(UniverseSeed));

            file.Close();

            Console.WriteLine("Universe Seed");
            Console.WriteLine(universeSeed.timeCycles);


            InitializeUniverse();

         }

        public static void InitializeUniverse()
        {
    
            for (int i = 0; i < universeSeed.particleCount; i++)
            {
                var particle = new Gerticle(); 
                particle.Position.X = (float)particle.GetUPos().X;
                particle.Position.Y = (float)particle.GetUPos().Y;
                particle.Position.Z = (float)particle.GetUPos().Z;
                particle.CalculateNormals();
                Universe.Particles.Add(new Gerticle());
            }

        }

        public static void StartUniverse()
        {
            double simTime = universeSeed.timeCycles;
            double dt;
            var loopTime = DateTime.Now;
            while(simTime > 0)
            {
                dt = (DateTime.Now - loopTime).TotalMilliseconds;
                loopTime = DateTime.Now;

              //  Console.WriteLine("UNIVERSE IS OPERATIONAL");
               // Console.WriteLine(dt);
                Universe.Update(dt/100);
                simTime -= dt/1000;
            }
            DestroyUniverse();
        }

        public static void DestroyUniverse()
        {
            //Calculate a better seed file initializer..

            UniverseSeed betterSeedFile = new UniverseSeed();
            //Gradient Decent on optimizing the generation of life..

            betterSeedFile.forces = 4;

            betterSeedFile.timeCycles = 100000;

            betterSeedFile.fieldVariants = 1;
            betterSeedFile.fieldPropportion = 1;
            betterSeedFile.fieldCount = 1;

            betterSeedFile.particleCount = 100;
            betterSeedFile.particleProportion = 0.4;
            betterSeedFile.particleVariants = 4;

            betterSeedFile.waveCount = 1;
            betterSeedFile.waveProportion = 1;
            betterSeedFile.waveVariants = 1;



            // serialize JSON to a string and then write string to a file
            File.WriteAllText(@"SeedFile/" + seedFileName + ".json", JsonConvert.SerializeObject(betterSeedFile));

      

     }
    }
}
