using _2DSpriteGameDX.GameLib.Perses;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2DSpriteGameDX.GameLib.Bonuses
{
    public class SpeedPlus : Bonus, ISuperPower
    {
        public SpeedPlus(Sprite sprite, Vector2 pos, int time = 10) : base(sprite, pos, time)
        {
            _bonusType = BonusType.SpeedPlus;
        }
        public Pers SuperPower(Pers pers)
        {
            return new FastPers(pers);
        }
    }

    
    
}
