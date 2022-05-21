using SharpDX;
using SharpDX.Direct2D1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2DSpriteGameDX
{
    public enum Traectory
    {
        upDown,
        leftRight, 
        circle, 
        fullRectangle
    }
    class Enemy 
    {
        private Dictionary<Direction, Sprite> _sprites;
        public Dictionary<Direction, Sprite> sprites { get => _sprites; set => _sprites = value; }

        public Sprite ActiveSprite { get => _activeSprite; }
        protected Sprite _activeSprite { get => _sprites[_direction]; }

        Traectory _traectory;

        Vector2 _startPos;
        Vector2 _finishPos;

        bool _toFinish;

        int _distance;

        RectangleF _purview;
        public RectangleF Purview { get => _purview; }

        protected Direction _direction;
        public Direction direction { get => _direction; set => _direction = value; }

        protected float _speed;
        public float Speed { get => _speed; set => _speed = value; }
        public RectangleF rect { get => new RectangleF(_left, _top, _activeSprite.Width, _activeSprite.Heigth); }

        protected int _left { get => (int)(_positionOfCenter.X - _activeSprite.Center.X); }
        protected int _top { get => (int)(_positionOfCenter.Y - _activeSprite.Center.Y); }
        private Vector2 _positionOfCenter;
        public Vector2 PositionOfCenter { get => _positionOfCenter; set => _positionOfCenter = value; }

        float s;
        public Enemy(Dictionary<Direction, Sprite> sprites, Traectory traectory, Vector2 start, int length, float speed)
        {
            _sprites = new Dictionary<Direction, Sprite>();
            foreach(var _el in sprites)
            {
                _sprites.Add(_el.Key, _el.Value);
            }
            _traectory = traectory;
            _startPos = start;
            switch (traectory)
            {
                case Traectory.leftRight:
                    {
                        Vector2 pos = start;
                        pos.X += length;
                        _finishPos = pos;
                        break;
                    }
                case Traectory.upDown:
                    {
                        Vector2 pos = start;
                        pos.Y += length;
                        _finishPos = pos;
                        break;
                    }
            }
            if (length < 0)
            {
                Vector2 pos;
                pos = _startPos;
                _startPos = _finishPos;
                _finishPos = pos;
                _toFinish = false;
                _positionOfCenter = _finishPos;
            }
            else
            {
                _toFinish = true;
                _positionOfCenter = _startPos;
            }
            _speed = speed;
            _distance = Math.Abs(length);
            s = 0;

        }

        public void Draw()
        {
            Vector2 pos = _positionOfCenter;
            switch (_traectory)
            {
                case Traectory.upDown:
                    {
                        if (_toFinish) { pos.Y += _speed; _direction = Direction.Ahead; }
                        else { pos.Y -= _speed; _direction = Direction.Back; }
                            
                        if (pos.Y > _finishPos.Y)
                        {
                            _toFinish = false; 
                            _direction = Direction.Back;
                        }
                        else if (pos.Y < _startPos.Y)
                        {
                            _toFinish = true; 
                            _direction = Direction.Ahead;
                        }
                        break;
                    }

                case Traectory.leftRight:
                    {
                        if (_toFinish) { pos.X += _speed; _direction = Direction.Rigth; }
                        else { pos.X -= _speed; _direction = Direction.Left; }
                        if (pos.X > _finishPos.X)
                        {
                            _toFinish = false;
                            _direction = Direction.Left;
                        }
                        else if (pos.X < _startPos.X)
                        {
                            _toFinish = true;
                            _direction = Direction.Rigth;
                        }
                        break;
                    }
                case Traectory.circle:
                    {

                        if (Math.Ceiling((double)s/(double)_distance) == 1 )
                        {
                            pos.Y += _speed;
                            _direction = Direction.Ahead;
                        }
                        else if(Math.Ceiling((double)s / (double)_distance) == 2)
                        {
                            pos.X += _speed;
                            _direction = Direction.Rigth;
                        }
                        else if(Math.Ceiling((double)s / (double)_distance) == 3)
                        {
                            pos.Y -= _speed;
                            _direction = Direction.Back;
                        }
                        else if(Math.Ceiling((double)s / (double)_distance) == 4)
                        {
                            pos.X -= _speed;
                            _direction = Direction.Left;
                        }
                        if (s > _distance * 4)
                        {
                            s = 0;
                        } 
                        s += _speed;
                        break;
                    }
            }

            _positionOfCenter = pos;
            float _purLeft = _left;
            float _purTop = _top;
            float _purWidth = (int)_activeSprite.Width;
            float _purHeigth = (int)_activeSprite.Heigth;
            switch (_direction)
            {
                case Direction.Ahead:
                    _purTop += (int)_activeSprite.Heigth;
                    _purHeigth *= 1.5f;
                    break;
                case Direction.Back:
                    _purTop -= (int)(_activeSprite.Heigth * 1.5);
                    _purHeigth *= 1.5f;
                    break;
                case Direction.Left:
                    _purLeft -= _activeSprite.Width * 3.0f;
                    _purWidth *= 3.0f;
                    break;
                case Direction.Rigth:
                    _purLeft += _activeSprite.Width;
                    _purWidth *= 3.0f;
                    break;
            }
            _purview = new RectangleF(_purLeft, _purTop , _purWidth , _purHeigth);
            _activeSprite.Draw(rect);
            _activeSprite.DrawFlash(_purview);
        }
    }
}
