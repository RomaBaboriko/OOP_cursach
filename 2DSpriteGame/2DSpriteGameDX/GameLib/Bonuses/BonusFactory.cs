using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2DSpriteGameDX.GameLib.Bonuses
{
    public class BonusFactory
    {
        DX2D _dx2d;
        public BonusFactory(DX2D dx2d)
        {
            _dx2d = dx2d;
        }
        public Bonus CreateBonus(BonusType i, Vector2 pos)
        {
            switch (i)
            {
                case BonusType.SpeedMinus:
                    {
                        return new SpeedMinus(new Sprite(_dx2d, _dx2d.BonusBitmaps[BonusType.SpeedMinus], BonusType.SpeedMinus.ToString(), 80), pos);
                    }
                case BonusType.SpeedPlus:
                    {
                        return new SpeedPlus(new Sprite(_dx2d, _dx2d.BonusBitmaps[BonusType.SpeedPlus], BonusType.SpeedPlus.ToString(), 50f), pos);
                    }
                case BonusType.TimeMinus:
                    {
                        return new TimeMinus(new Sprite(_dx2d, _dx2d.BonusBitmaps[BonusType.TimeMinus], BonusType.TimeMinus.ToString(), 50), pos);
                    }
                case BonusType.TimePlus:
                    {
                        return new TimePlus(new Sprite(_dx2d, _dx2d.BonusBitmaps[BonusType.TimePlus], BonusType.TimePlus.ToString(), 20f), pos);
                    }
                case BonusType.Protection:
                    {
                        return new Protection(new Sprite(_dx2d, _dx2d.BonusBitmaps[BonusType.Protection], BonusType.Protection.ToString(), 40f), pos);
                    }
                case BonusType.Freezing:
                    {
                        return new Freezing(new Sprite(_dx2d, _dx2d.BonusBitmaps[BonusType.Freezing], BonusType.Freezing.ToString(), 20), pos);
                    }
                case BonusType.Key:
                    {
                        return new Key(new Sprite(_dx2d, _dx2d.BonusBitmaps[BonusType.Key], BonusType.Freezing.ToString(), 40), pos);
                    }
                default:
                    {
                        return new Freezing(new Sprite(_dx2d, _dx2d.BonusBitmaps[BonusType.Freezing], BonusType.Freezing.ToString(), 10), pos);
                    }
            }
        }
    }
}
