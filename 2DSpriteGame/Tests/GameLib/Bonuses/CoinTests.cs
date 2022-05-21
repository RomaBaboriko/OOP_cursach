using Microsoft.VisualStudio.TestTools.UnitTesting;
using _2DSpriteGameDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;

namespace _2DSpriteGameDX.Tests
{
    [TestClass()]
    public class CoinTests
    {
        [TestMethod()]
        public void TakeCoinTest()
        {
            Coin coin = new Coin(new Vector2(0,0));
            coin.TakeCoin();
            bool expected = true;
            bool test = coin.IsTaken;
            Assert.IsTrue(test == expected);
        }
    }
}