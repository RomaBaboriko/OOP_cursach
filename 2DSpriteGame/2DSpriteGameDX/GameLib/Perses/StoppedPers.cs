using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2DSpriteGameDX.GameLib.Perses
{
    public class StoppedPers : SuperPers
    {
        public bool Stopped;
        public StoppedPers(Pers pers) : base(pers)
        {
            Stopped = true;
            direction = Direction.Ahead;
        }
        public override void MoveLeft() { }
        public override void MoveRigth() { }
        public override void MoveUp() { }
        public override void MoveDown() { }
    }
}
