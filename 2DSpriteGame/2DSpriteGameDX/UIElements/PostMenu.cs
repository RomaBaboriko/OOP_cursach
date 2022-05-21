using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _2DSpriteGameDX
{
    class PostMenu
    {
        RenderForm _renderForm;
        DX2D _dx2d;
        DInput _dInput;
        WindowRenderTarget _target;
        Size2F _targetSize;
        RectangleF _messageButton;
        RectangleF rec;
        public string _message;

        float scale;
        int titleHeight;

        int _messageLeft;
        int _messageTop;

        int _messageWidth;
        int _messageHeight;

        bool _messagePressed;
        public bool _closeWindow;
        // Создание имитированных кнопочек
        public PostMenu(RenderForm renderForm, DX2D dx2d, WindowRenderTarget target, Size2F targetSize, DInput dInput)
        {

            scale = target.Size.Width / target.PixelSize.Width;
            _renderForm = renderForm;
            _dx2d = dx2d;
            _dInput = dInput;
            _target = target;
            _targetSize = targetSize;
            _target.Transform = Matrix3x2.Identity * scale;
            _targetSize.Height /= scale;
            _targetSize.Width /= scale;


            _messageWidth = Convert.ToInt32(_targetSize.Width * 0.45);
            _messageHeight = Convert.ToInt32(_targetSize.Height * 0.15);

            _messageLeft = Convert.ToInt32(_targetSize.Width / 2 - _messageWidth / 2);
            _messageTop = Convert.ToInt32(_targetSize.Height / 2 - _messageHeight /2);

            _messageButton.X = _messageLeft;
            _messageButton.Y = _messageTop;
            _messageButton.Width = _messageWidth;
            _messageButton.Height = _messageHeight;

            _messagePressed = false;

            _closeWindow = false;

            System.Drawing.Rectangle screenRectangle = _renderForm.RectangleToScreen(_renderForm.ClientRectangle);
            titleHeight = screenRectangle.Top - _renderForm.Top;
        }

        //Отрисовка
        public void Draw()
        {
            _target.Transform = Matrix3x2.Identity * scale;

            RectangleF _messageButtonText = _messageButton;
            //startButtonText.X = startButton.X + (startButton.X - startText.Length * _dx2d.TextFormatStats.FontSize) / 4;

           
            _target.FillRectangle(_messageButtonText, new SolidColorBrush(_target, new Color(255, 255, 255)));
            _target.DrawText(_message,
                _dx2d.TextFormatStats, _messageButtonText, new SolidColorBrush(_target, new Color(0, 0, 0)));
        }

        //Проверка на нажатие по кнопочке
        public void StartOnClick()
        {
            rec = new RectangleF((Cursor.Position.X - ((_renderForm.Location.X < 0) ? 0 : _renderForm.Location.X)), (Cursor.Position.Y - _renderForm.Location.Y) - titleHeight, 20, 20);
            //_target.FillRectangle(rec, _dx2d.BlueBrush);


            if (rec.Intersects(_messageButton))
            {
                if (_dInput.MouseState.Buttons[0])
                {
                    _messagePressed = true;
                }
                if (!_dInput.MouseState.Buttons[0] && _messagePressed)
                {
                    _closeWindow = true;
                    _messagePressed = false;
                }
            }

        }

        //рендер каждый кадр
        public void Update()
        {
            Draw();
            _dInput.UpdateMouseState();
            if (_dInput.MouseUpdated)
            {
                StartOnClick();
            }
        }
    }
}
