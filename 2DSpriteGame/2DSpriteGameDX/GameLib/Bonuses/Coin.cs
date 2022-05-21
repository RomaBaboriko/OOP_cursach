using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2DSpriteGameDX
{
    public class Coin
    {
        Sprite _sprite;
        bool _isTaken;
        public bool IsTaken { get => _isTaken; }
        public RectangleF rect { get => new RectangleF(_left, _top, _sprite.Width, _sprite.Heigth); }

        protected int _left { get => (int)(_positionOfCenter.X - _sprite.Center.X); }
        protected int _top { get => (int)(_positionOfCenter.Y - _sprite.Center.Y); }
        private Vector2 _positionOfCenter;
        public Vector2 PositionOfCenter { get => _positionOfCenter; set => _positionOfCenter = value; }
        public Coin(Sprite sprite, Vector2 pos) 
        {
            _sprite = sprite;
            _positionOfCenter = pos;
            _isTaken = false;
            sprite._scale = 15;
            sprite.GetWidthHeigth();
        }
        public Coin(Vector2 pos)
        {
            _positionOfCenter = pos;
            _isTaken = false;
        }
        public void Draw()
        {
            _sprite.Draw(rect);
        }
        public void TakeCoin()
        {
            _isTaken = true;
        }
    }
}
