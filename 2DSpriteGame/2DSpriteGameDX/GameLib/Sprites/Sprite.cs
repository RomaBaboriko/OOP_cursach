using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using SharpDX.Direct2D1;

namespace _2DSpriteGameDX
{
    public enum Direction
    {
        Left,
        Back,
        Rigth,
        Ahead,
    }

    public class Sprite
    {
        string _name;

        //private Dictionary<string, SharpDX.Direct2D1.Bitmap> _bitmaps;
        //public Dictionary<string, SharpDX.Direct2D1.Bitmap> Bitmaps { get => _bitmaps; }
        private SharpDX.Direct2D1.Bitmap _bitmap;
        public SharpDX.Direct2D1.Bitmap bitmap { get => _bitmap; }
        //public Bitmap ActiveBitmap { get => _activeBitmap; set => _activeBitmap = value; }
        //protected Bitmap _activeBitmap;
        // 16 пикселей текстуры соответствуют единице игрового пространства
        //private static readonly float _pu = 1.0f / 16.0f;
        public float _scale;
        protected DX2D _dx2d;

        protected float _width;
        protected float _height;
        public float Width { get => _width; }
        public float Heigth { get => _height; }

        protected Vector2 _center;
        public Vector2 Center { get => _center; }
        public Sprite(DX2D dx2d, string path, string name) : this(dx2d, dx2d.GetBitmap(path + name + ".bmp"), name)
        {
            Bitmap bitmap = dx2d.GetBitmap(path + name + ".bmp");
        }

        public Sprite(DX2D dx2d, Bitmap bitmap, string name, float scale = 1.3f)
        {

            _name = name;
            _dx2d = dx2d;

            _bitmap = bitmap;

            _scale = scale;

            _width = _bitmap.Size.Width / _scale;
            _height = _bitmap.Size.Height / _scale;
        }

        public void GetWidthHeigth()
        {
            // _width = _bitmaps["Left"].Size.Width / _scale;
            //_height = _bitmaps["Left"].Size.Height / _scale;
            _width = _bitmap.Size.Width / _scale;
            _height = _bitmap.Size.Height / _scale;
        }

        // Отрисовка
        public virtual void Draw(RectangleF rect)
        {
            if (_bitmap == null) throw new Exception("bitmap was null");
            _dx2d.RenderTarget.DrawBitmap(_bitmap,
                                       rect,
                                       1,
                                       SharpDX.Direct2D1.BitmapInterpolationMode.Linear);
        }
        public void DrawFlash(RectangleF rect)
        {
            _dx2d.RenderTarget.FillRectangle(rect, _dx2d.SemiLightBrush);
        }
        public void DrawProtection(RectangleF rect)
        {
            _dx2d.RenderTarget.DrawRectangle(rect, _dx2d.BlueBrush);
        }
    }
}
