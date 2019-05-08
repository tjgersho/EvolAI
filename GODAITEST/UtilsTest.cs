using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using GodAIAPI;


namespace GODAITEST
{
    [TestClass]
    public class UtilsTest
    {
        [TestMethod]
        public void RandomMintoMax()
        {
            var val = Utils.GetRandomBetween(10, 100);
            Console.WriteLine(val);

            val = Utils.GetRandomBetween(1, 10);
            Console.WriteLine(val);
        }
    }
}
