using Microsoft.VisualStudio.TestTools.UnitTesting;
using _2DSpriteGameDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using SharpDX.Windows;

namespace _2DSpriteGameDX.Tests
{
    [TestClass()]
    public class PersTests
    {
        [TestMethod()]
        public void MoveLeftTest()
        {
            Pers pers = new Pers(new Vector2(1450f, 350f), 2.3f, "Jonh");
            Vector2 pos = pers.PositionOfCenter;
            pers.MoveLeft();
            Assert.IsTrue((pos.X - pers.Speed) == pers.PositionOfCenter.X);
        }

        [TestMethod()]
        public void MoveRigthTest()
        {
            Pers pers = new Pers(new Vector2(1450f, 350f), 2.3f, "Jonh");
            Vector2 pos = pers.PositionOfCenter;
            pers.MoveRigth();
            Assert.IsTrue((pos.X + pers.Speed) == pers.PositionOfCenter.X);
        }

        [TestMethod()]
        public void MoveUpTest()
        {
            Pers pers = new Pers(new Vector2(1450f, 350f), 2.3f, "Jonh");
            Vector2 pos = pers.PositionOfCenter;
            pers.MoveUp();
            Assert.IsTrue((pos.Y - pers.Speed) == pers.PositionOfCenter.Y);
        }

        [TestMethod()]
        public void MoveDownTest()
        {
            Pers pers = new Pers(new Vector2(1450f, 350f), 2.3f, "Jonh");
            Vector2 pos = pers.PositionOfCenter;
            pers.MoveDown();
            Assert.IsTrue((pos.Y + pers.Speed) == pers.PositionOfCenter.Y);
        }
    }
}