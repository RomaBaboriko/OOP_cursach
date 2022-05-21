using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2DSpriteGameDX.GameLib.Perses
{
    public abstract class SuperPers : Pers
    {
        public Pers pers;
        public SuperPers(Pers pers) : base(pers.sprites, pers.PositionOfCenter, pers.Speed, pers.Name)
        {
            this.pers = pers;
            this.direction = pers.direction;
        }
    }
    public class FastPers : SuperPers
    {
        public FastPers(Pers pers) : base(pers)
        {
            this._speed *= 3;
        }
    }
    public class KeyKeeper : SuperPers
    {
        public bool HaveKey;
        Sprite key;
        public KeyKeeper(Pers pers, Sprite key) : base(pers)
        {
            HaveKey = true;
            this.key = key;
        }
        public override void Draw()
        {
            float _purLeft = _left;
            float _purTop = _top;
            float _purWidth = (int)_activeSprite.Width;
            float _purHeigth = (int)_activeSprite.Heigth;
            switch (_direction)
            {
                case Direction.Ahead:
                    _purTop += (int)_activeSprite.Heigth;
                    break;
                case Direction.Back:
                    _purTop -= (int)(_activeSprite.Heigth * 1);
                    break;
                case Direction.Left:
                    _purLeft -= _activeSprite.Width * 1;
                    break;
                case Direction.Rigth:
                    _purLeft += _activeSprite.Width;
                    break;
            }
            base.Draw();
            key.Draw(new RectangleF(_purLeft, _purTop, _purWidth, _purHeigth));

        }
    }
}
