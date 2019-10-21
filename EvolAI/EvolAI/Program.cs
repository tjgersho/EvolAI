using System;
using System.Threading.Tasks;
using GodAIAPI;
using GodAIAPI.UI;

namespace GodAI
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("---Hello World!---");

            GOD.SeedUniverse();

            Task.Run(() =>
            {
                GOD.StartUniverse();
            });

        
            using (var window = new Window(GOD.Universe))
            {
                window.Run(60);
            }
    
            Console.ReadKey();

        }
    }
}
