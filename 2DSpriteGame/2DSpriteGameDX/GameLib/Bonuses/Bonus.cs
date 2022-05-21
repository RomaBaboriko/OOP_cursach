using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2DSpriteGameDX
{

    public enum BonusType
    {
        TimePlus,
        TimeMinus, 
        SpeedPlus,
        SpeedMinus,
        Freezing,
        Protection,
        Key,
    }

    public interface ISuperPower
    {
        Pers SuperPower(Pers pers);
    }

    public abstract class Bonus
    {
        protected Sprite _sprite;
        bool _isTaken;
        int _time;
        public bool IsUsed { get => _currentTime >= _time ? true : false; }
        public int Time { get => _time; }
        protected int _currentTime;
        public int CurrentTime { get => _currentTime; set => _currentTime = value; }
        public bool IsTaken { get => _isTaken; }
        public RectangleF rect { get => new RectangleF(_left, _top, _sprite.Width, _sprite.Heigth); }

        protected int _left { get => (int)(_positionOfCenter.X - _sprite.Center.X); }
        protected int _top { get => (int)(_positionOfCenter.Y - _sprite.Center.Y); }
        private Vector2 _positionOfCenter;
        public Vector2 PositionOfCenter { get => _positionOfCenter; set => _positionOfCenter = value; }
        protected BonusType _bonusType; 
        public BonusType bonusType { get => _bonusType; }
        public Bonus(Sprite sprite, Vector2 pos, int time)
        {
            _sprite = sprite;
            _positionOfCenter = pos;
            _isTaken = false;
            sprite.GetWidthHeigth();
            _time = time;
            _currentTime = 0;

        }
        public void Draw()
        {
            _sprite.Draw(rect);
        }
        public void TakeBonus()
        {
            _isTaken = true;
        }

    }
}
