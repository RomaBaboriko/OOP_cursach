using _2DSpriteGameDX.GameLib.Perses;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2DSpriteGameDX.GameLib.Bonuses
{
    public class Protection : Bonus, ISuperPower
    {
        public Protection(Sprite sprite, Vector2 pos, int time = 10) : base(sprite, pos, time)
        {
            _bonusType = BonusType.Protection;
        }

        public Pers SuperPower(Pers pers)
        {
            return new ProtectedPers(pers);
        }
    }
}

