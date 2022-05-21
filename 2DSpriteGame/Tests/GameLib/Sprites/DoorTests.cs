using Microsoft.VisualStudio.TestTools.UnitTesting;
using _2DSpriteGameDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2DSpriteGameDX.Tests
{
    [TestClass()]
    public class DoorTests
    {
        [TestMethod()]
        public void OpenTest()
        {
            Door door = new Door(false);
            door.Open();

            Assert.IsTrue(!door._isLocked);
        }
    }
}