using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using GodAIAPI;
using GodAIAPI.Utils;


namespace GODAITEST
{
    [TestClass]
    public class UtilsTest
    {
        [TestMethod]
        public void RandomMintoMax()
        {
            var val = GodAIUtils.GetRandomBetween(10, 100);
            Console.WriteLine(val);

            val = GodAIUtils.GetRandomBetween(1, 10);
            Console.WriteLine(val);
        }
    }
}
