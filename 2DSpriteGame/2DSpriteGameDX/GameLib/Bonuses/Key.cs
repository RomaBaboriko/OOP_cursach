using _2DSpriteGameDX.GameLib.Perses;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2DSpriteGameDX.GameLib.Bonuses
{
    public class Key : Bonus, ISuperPower
    {
        public Key(Sprite sprite, Vector2 pos, int time = 60) : base(sprite, pos, time)
        {
            _bonusType = BonusType.Key;
        }
        public Pers SuperPower(Pers pers)
        {

            return new KeyKeeper(pers, _sprite);
        }
    }
}
