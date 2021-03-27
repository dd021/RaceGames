using Microsoft.VisualStudio.TestTools.UnitTesting;
using RaceGames;
using System;

namespace FactoryTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            Player player = PlayerFactory.GetPlayer(2);
            Assert.AreEqual(player is Bob, true);
        }
    }
}
