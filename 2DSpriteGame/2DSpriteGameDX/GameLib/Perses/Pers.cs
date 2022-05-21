using _2DSpriteGameDX;
using SharpDX;
using System;
using System.Collections.Generic;

namespace _2DSpriteGameDX
{
    public class Pers 
    {
        private Dictionary<Direction, Sprite> _sprites;
        public Dictionary<Direction, Sprite> sprites { get => _sprites; set => _sprites = value; }

        public Sprite ActiveSprite { get => _activeSprite; }
        protected Sprite _activeSprite { get => _sprites[_direction]; }

        //public Sprite sprite { get => _sprite; set => _sprite = value; }
        string _name;
        public string Name { get => _name; set => _name = value; }

        protected bool _isAlive;
        public bool IsAlive { get => _isAlive; set => _isAlive = value; }

        protected Direction _direction;
        public Direction direction { get => _direction; set => _direction = value; }

        protected float _speed;
        public float Speed { get => _speed; set => _speed = value; }

        public RectangleF rect { get => new RectangleF(_left, _top, _sprites[Direction.Ahead].Width, _sprites[Direction.Ahead].Heigth); }

        protected int _left { get => (int)(_positionOfCenter.X - _sprites[Direction.Ahead].Center.X); }
        protected int _top { get => (int)(_positionOfCenter.Y - _sprites[Direction.Ahead].Center.Y); }
        private Vector2 _positionOfCenter;
        public Vector2 PositionOfCenter { get => _positionOfCenter; set => _positionOfCenter = value; }

        public Pers(Dictionary<Direction, Sprite> sprites, Vector2 pos, float speed, string name = "Tom")
        {
            _positionOfCenter.X = pos.X;
            _positionOfCenter.Y = pos.Y;

            _sprites = new Dictionary<Direction, Sprite>();
            foreach (var _el in sprites)
            {
                _sprites.Add(_el.Key, _el.Value);
            }
            _name = name;

            _speed = speed;
            _isAlive = true;

            _direction = Direction.Left;
        }

        public Pers(Vector2 pos, float speed, string name = "Tom")
        {
            _positionOfCenter.X = pos.X;
            _positionOfCenter.Y = pos.Y;

            _name = name;

            _speed = speed;
            _isAlive = true;

            _direction = Direction.Left;
        }
        public virtual void Draw()
        {
            _activeSprite.Draw(rect);
        }

        public virtual void MoveLeft()
        {
            Vector2 pos = this._positionOfCenter;
            pos.X -= _speed;
            this._positionOfCenter = pos;
            _direction = Direction.Left;
        }
        public virtual void MoveRigth()
        {
            Vector2 pos = this._positionOfCenter;
            pos.X += _speed;
            this._positionOfCenter = pos;
            _direction = Direction.Rigth;
        }
        public virtual void MoveUp()
        {
            Vector2 pos = this._positionOfCenter;
            pos.Y -= _speed;
            this._positionOfCenter = pos;
            _direction = Direction.Back;
        }
        public virtual void MoveDown()
        {
            Vector2 pos = this._positionOfCenter;
            pos.Y += _speed;
            this._positionOfCenter = pos;
            _direction = Direction.Ahead;
        }

    }
}
