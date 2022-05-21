using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2DSpriteGameDX.GameLib.Perses
{
    public class SlowPers : SuperPers
    {
        public SlowPers(Pers pers) : base(pers)
        {
            this._speed /= 2;
        }
    }
}
