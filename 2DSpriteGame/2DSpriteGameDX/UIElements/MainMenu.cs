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
    class MainMenu
    {
        RenderForm _renderForm;
        DX2D _dx2d;
        DInput _dInput;
        WindowRenderTarget _target;
        Size2F _targetSize;
        RectangleF startButton;
        RectangleF rec;

        float scale;
        int titleHeight;

        public bool startGame;

        int startButtonLeft;
        int startButtonTop;

        int buttonWidth;
        int buttonHeight;

        bool _startButtonPressed;
        // Создание имитированных кнопочек
        public MainMenu(RenderForm renderForm, DX2D dx2d, WindowRenderTarget target, Size2F targetSize, DInput dInput)
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


            buttonWidth = Convert.ToInt32(_targetSize.Width * 0.25);
            buttonHeight = Convert.ToInt32(_targetSize.Height * 0.15);

            startButtonLeft = Convert.ToInt32(_targetSize.Width / 2 - buttonWidth / 2);
            startButtonTop = Convert.ToInt32(_targetSize.Height / 2 - buttonHeight / 2);

            startButton.X = startButtonLeft;
            startButton.Y = startButtonTop;
            startButton.Width = buttonWidth;
            startButton.Height = buttonHeight;

            _startButtonPressed = false;

            startGame = false;

            System.Drawing.Rectangle screenRectangle = _renderForm.RectangleToScreen(_renderForm.ClientRectangle);
            titleHeight = screenRectangle.Top - _renderForm.Top;
        }

        //Отрисовка
        public void DrawMainMenu()
        {
            _target.Transform = Matrix3x2.Identity * scale;

            RectangleF title = new RectangleF();
            string titleText = "Bank";

            title.Width = Convert.ToInt32(_targetSize.Width * 0.4);
            title.Height = Convert.ToInt32(_targetSize.Height * 0.10);

            title.X = Convert.ToInt32(_targetSize.Width / 2 - title.Width / 2);
            title.Y = Convert.ToInt32(_targetSize.Height * 0.2);

            RectangleF startButtonText = startButton;
            string startText = "START";
            //startButtonText.X = startButton.X + (startButton.X - startText.Length * _dx2d.TextFormatStats.FontSize) / 4;

            _target.DrawText(titleText,
                _dx2d.TextFormatTitle, title, new SolidColorBrush(_target, new Color(0, 0, 0)));

            _target.FillRectangle(startButton, new SolidColorBrush(_target, new Color(255, 255, 255)));
            _target.DrawText(startText,
                _dx2d.TextFormatStats, startButtonText, new SolidColorBrush(_target, new Color(0, 0, 0)));
        }

        //Проверка на нажатие по кнопочке
        public void StartOnClick()
        {
            rec = new RectangleF((Cursor.Position.X - ((_renderForm.Location.X < 0) ? 0 : _renderForm.Location.X)), (Cursor.Position.Y - _renderForm.Location.Y) - titleHeight, 20, 20);
            //_target.FillRectangle(rec, _dx2d.BlueBrush);


            if (rec.Intersects(startButton))
            {
                if (_dInput.MouseState.Buttons[0])
                {
                    _startButtonPressed = true;
                }
                if (!_dInput.MouseState.Buttons[0] && _startButtonPressed)
                {
                    startGame = true;
                    _startButtonPressed = false;
                }
            }

        }

        //рендер каждый кадр
        public void UpdateMainMenu()
        {
            DrawMainMenu();
            _dInput.UpdateMouseState();
            if (_dInput.MouseUpdated)
            {
                StartOnClick();
            }
        }
    }
}
