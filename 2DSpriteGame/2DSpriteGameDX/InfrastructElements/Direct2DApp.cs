using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.Windows;
using SharpDX.DirectInput;
using System.Windows.Forms;
using _2DSpriteGameDX.GameLib.Perses;

namespace _2DSpriteGameDX
{
    public class Direct2DApp : IDisposable
    {
        // Для расчета коэффициента масштабирования принимаем, что вся область окна по высоте вмещает 20 единиц длинны виртуального игрового пространства
        private static float _unitsPerHeight = 20.0f;
        public static float UnitsPerHeight { get => _unitsPerHeight; }

        // Окно программы
        private RenderForm _renderForm;
        public RenderForm RenderForm { get => _renderForm; }

        // Инфраструктурные объекты
        private DX2D _dx2d;
        public DX2D DX2D { get => _dx2d; }
        private DInput _dInput;

        // Клиетская область порта отрисовки в устройство-независимых пикселях
        private RectangleF _clientRect;

        // Коэффициент масштабирования
        private float _scale;
        public float Scale { get => _scale; }

        // Помощник для работы со временем
        private TimeHelper _timeHelper;

        // Спрайт фона
        private BackGround _background;
        private BackGround _background2;
        private float _backgroundscale;
        private float _background2scale;
        int backgroundIndex;
        int background2Index;
        // Спрайты персонажей и врагов
        private Pers _first;
        public Pers First { get => _first; }
        private Pers _second;
        public Pers Second { get => _second; }
        private List<Enemy> _enemies;

        // булевая переменная отвечающая за окончание игры
        bool _gameEnded;

        // строка, содержащая информацию о результате игры
        string _resultOfGame;
        // Цель отрисовки
        WindowRenderTarget _target;
        Size2F _targetSize;

        Random rnd = new Random();

        // коллекция монет на поле
        List<Coin> _coins;
        public List<Coin> Coins { get => _coins; }

        //коллекция бонусов на поле
        List<Bonus> _bonuses;

        //коллекция используемых бонусов
        Dictionary<Pers, Bonus> _activeBonuses;

        // меню начала игры
        MainMenu _mainMenu;

        // окно вывода результатов игры
        PostMenu _postMenu;

        // коллекция стен
        List<RectangleF> _walls;

        // коллекция дверей
        List<Door> _doors;

        // время игровой сессии
        int _gameTime;

        // прошедшее время
        int _currentTime;

        // количество монет на поле
        int _coins_count;
        // количество взятых монеток
        int _takenCoins { get => GetTakenCoins(); }
        // В конструкторе создаем форму, инфраструктурные объекты, подгружаем спрайты, создаем помощник для работы со временем
        // В конце дергаем ресайзинг формы для вычисления масштаба и установки пределов по горизонтали и вертикали
        public Direct2DApp()
        {
            _renderForm = new RenderForm("Direct2D Application");
            _renderForm.WindowState = FormWindowState.Maximized;
            _renderForm.IsFullscreen = true;

            _dx2d = new DX2D(_renderForm);
            _dInput = new DInput(_renderForm);

            _target = _dx2d.RenderTarget;
            _targetSize = _target.Size;

            backgroundIndex = _dx2d.LoadBitmap("..\\..\\img\\bg1up.bmp");
            background2Index = _dx2d.LoadBitmap("..\\..\\img\\Background\\bg2up.bmp");
            _background = new BackGround(_dx2d, backgroundIndex, 0.0f, 0.0f, 0.0f);
            _background2 = new BackGround(_dx2d, background2Index, 0.0f, 0.0f, 0.0f);
            _backgroundscale = 1.4f;
            _background2scale = 1.4f;

            _mainMenu = new MainMenu(_renderForm, _dx2d, _target, _targetSize, _dInput);
            _postMenu = new PostMenu(_renderForm, _dx2d, _target, _targetSize, _dInput);

            //int pers1Index = _dx2d.LoadBitmap("D:\\Styding\\4 семестр\\bank\\2DSpriteGame\\2DSpriteGameDX\\img\\player1.bmp");

            Dictionary<Direction, Sprite> sprites = new Dictionary<Direction, Sprite>();

            string _firstPath = "..\\..\\img\\Pers1\\";
            foreach(Direction dir in Enum.GetValues(typeof(Direction)))
            {
                sprites.Add(dir, new Sprite(_dx2d, _firstPath, "sprite1" + dir.ToString()));
            }
            _first = new Pers(sprites, new Vector2(1450f, 350f), 2.3f, "Jonh");

            sprites.Clear();
            //int pers2Index = _dx2d.LoadBitmap("D:\\Styding\\4 семестр\\bank\\2DSpriteGame\\2DSpriteGameDX\\img\\player2.bmp");
            string _secondPath = "..\\..\\img\\Pers2\\";
            foreach (Direction dir in Enum.GetValues(typeof(Direction)))
            {
                sprites.Add(dir, new Sprite(_dx2d, _secondPath, "sprite2" + dir.ToString()));
            }
            _second = new Pers(sprites, new Vector2(1450f, 350f), 2.3f, "Mark");
            sprites.Clear();

            _enemies = new List<Enemy>();
           // _enemyscale = 0.4f;

            string enemyPath = "..\\..\\img\\Enemy\\";

            foreach (Direction dir in Enum.GetValues(typeof(Direction)))
            {
                sprites.Add(dir, new Sprite(_dx2d, enemyPath, "enemy" + dir.ToString()));
            }

            _enemies.Add(new Enemy(sprites, Traectory.circle, new Vector2(220, 240), 300, 0.9f));
            _enemies.Add(new Enemy(sprites, Traectory.leftRight, new Vector2(600, 60), 460, 0.9f));
            _enemies.Add(new Enemy(sprites, Traectory.upDown, new Vector2(1200, 125), 415, 0.9f));
            _enemies.Add(new Enemy(sprites, Traectory.leftRight, new Vector2(700, 660), 360, 0.9f));
           _enemies.Add(new Enemy(sprites, Traectory.upDown, new Vector2(1140, 560), -415, 0.9f));

            sprites.Clear();

            _walls = new List<RectangleF>();
            _walls.Add(new RectangleF(0, 0, 596, 144));
            _walls.Add(new RectangleF(0, 114, 198, 638));
            _walls.Add(new RectangleF(0, 732, 516, 100));
            _walls.Add(new RectangleF(495, 634, 20, 120));
            _walls.Add(new RectangleF(497, 633, 200, 220));
            _walls.Add(new RectangleF(675, 634, 20, 120));
            _walls.Add(new RectangleF(677, 732, 438, 120));
            _walls.Add(new RectangleF(1095, 634, 20, 120));
            _walls.Add(new RectangleF(1095, 633, 220, 320));
            _walls.Add(new RectangleF(1294, 435, 20, 218));
            _walls.Add(new RectangleF(1294, 436, 421, 720));
            _walls.Add(new RectangleF(1394, 217, 20, 85));
            _walls.Add(new RectangleF(1394, 367, 20, 85));
            _walls.Add(new RectangleF(1295, 219, 121, 22));
            _walls.Add(new RectangleF(1294, 116, 420, 127));
            _walls.Add(new RectangleF(1095, 114, 220, 30));
            _walls.Add(new RectangleF(294, 514, 100, 20));
            _walls.Add(new RectangleF(1095, 0, 536, 128));
            _walls.Add(new RectangleF(577, 0, 538, 43));
            _walls.Add(new RectangleF(577, 16, 20, 128));
            _walls.Add(new RectangleF(294, 334, 20, 200));
            _walls.Add(new RectangleF(377, 414, 120, 20));
            _walls.Add(new RectangleF(377, 416, 20, 118));
            _walls.Add(new RectangleF(387, 334, 110, 20));
            _walls.Add(new RectangleF(477, 335, 20, 100));
            _walls.Add(new RectangleF(477, 335, 20, 100));
            _walls.Add(new RectangleF(796, 516, 100, 20));
            _walls.Add(new RectangleF(796, 416, 20, 118));
            _walls.Add(new RectangleF(695, 416, 119, 20));
            _walls.Add(new RectangleF(695, 334, 20, 104));
            _walls.Add(new RectangleF(695, 334, 402, 20));
            _walls.Add(new RectangleF(1075, 334, 20, 100));
            _walls.Add(new RectangleF(1075, 334, 20, 100));
            _walls.Add(new RectangleF(877, 416, 220, 20));

            _walls.Add(new RectangleF(0, 0, 1, 800));
            _walls.Add(new RectangleF(0, 0, 1500, 1));
            _walls.Add(new RectangleF(1500, 0, 1501, 800));
            _walls.Add(new RectangleF(0, 800, 1501, 801));

            _doors = new List<Door>();
            _doors.Add(new Door(new RectangleF(314, 334, 75, 20), true));
            _doors.Add(new Door(new RectangleF(877, 436, 20, 75), true));
            _doors.Add(new Door(new RectangleF(1394, 296, 20, 75), false));
            foreach(BonusType bonusType in Enum.GetValues(typeof(BonusType)))
            {
                string path = "..\\..\\img\\CoinBonus\\";
                _dx2d.LoadBonusBitmap(path + bonusType.ToString() + ".bmp", bonusType);
            }

            _gameEnded = false;
            _timeHelper = new TimeHelper();
            RenderForm_Resize(this, null);

            _gameTime = 120;
            _currentTime = 0;

            _coins_count = 120;

            CreateCoins();
        }
        
        // Делегат, вызываемый для формирования каждого кадра
        private void RenderCallback()
        {
            // Дергаем обновление состояния "временного" помощника и объектов ввода
            _timeHelper.Update();
            _dInput.UpdateKeyboardState();
            _dInput.UpdateMouseState();

            // Обновляем время и счетчик кадров
            int fps = _timeHelper.FPS;
            float time = _timeHelper.Time;
            float dT = _timeHelper.dT;

            // Область просмотра в "прямом иксе 2 измерения" считается в "попугаях", т.е. в устройство-независимых пикселях несмотря на "dpiAware" в манифесте приложения
            // Поэтому для расчетов масштаба берем не клиентскую область формы, которая в честных пикселях, а RenderTarget-а
            //WindowRenderTarget target = _dx2d.RenderTarget;
            //Size2F targetSize = target.Size;
            _clientRect.Width = _targetSize.Width;
            _clientRect.Height = _targetSize.Height;


            // Начинаем вывод графики
            _target.BeginDraw();
            // Перво-наперво - очистить область отображения
            _target.Clear(SharpDX.Color.Black);

            Vector2 backgroundPos = new Vector2(0, 0);
            _background.PositionOfCenter = backgroundPos;
            //_background.DrawBackground(1.0f, _scale * _backgroundscale, _unitsPerHeight / 1080.0f, _clientRect.Height);
            //target.DrawRectangle(_first.rect, _dx2d.WhiteBrush);

            if (!_mainMenu.startGame)
            {
                _background2.DrawBackground(1.0f, _scale * _background2scale, _unitsPerHeight / 1080.0f, _clientRect.Height);
                _mainMenu.UpdateMainMenu();
            }
            else if (!_gameEnded)
            {
                _background.DrawBackground(1.0f, _scale * _backgroundscale, _unitsPerHeight / 1080.0f, _clientRect.Height);
                _dInput.UpdateKeyboardState();
                _dInput.UpdateMouseState();
                _target.DrawText($"Time : {_gameTime - _currentTime}", _dx2d.TextFormatScoreTime, 
                    new SharpDX.Mathematics.Interop.RawRectangleF((int)(_clientRect.Width / 2), 0, (int)(_clientRect.Width * 0.3), (int)(_clientRect.Height * 0.1)), _dx2d.WhiteBrush);

                GameRender();
            }
            else
            {
                _postMenu._message = "You are " + _resultOfGame;
                _postMenu.Update();
                if(_postMenu._closeWindow) RenderForm.Close();
            }
            
            if (!_first.IsAlive && !_second.IsAlive || _takenCoins == _coins_count || _currentTime > _gameTime)
            {
                GameEnd();
            }

            if (_timeHelper._isSec)
            {
                _currentTime++;
                if (ifCreateBonus())
                {
                    CreateBonus();
                }
            }
            _target.EndDraw();
            
        }

        // обновление всех элементов игры
        void GameRender()
        {
            DrawCoins();
            DrawBonuses();
            ApplyBonuses(ref _first);
            ApplyBonuses(ref _second);

            if (_first.IsAlive  && _second.IsAlive)
            {
                Vector2 pos1 = _first.PositionOfCenter;
                if (_dInput.KeyboardUpdated)
                {
                    bool wPressed = _dInput.KeyboardState.IsPressed(Key.W);
                    bool sPressed = _dInput.KeyboardState.IsPressed(Key.S);
                    if (sPressed && !IsIntrSmth(_first, Direction.Ahead)) _first.MoveDown();
                    if (wPressed && !IsIntrSmth(_first, Direction.Back)) _first.MoveUp();

                    bool aPressed = _dInput.KeyboardState.IsPressed(Key.A);
                    bool dPressed = _dInput.KeyboardState.IsPressed(Key.D);
                    if (dPressed && !IsIntrSmth(_first, Direction.Rigth)) _first.MoveRigth();
                    if (aPressed && !IsIntrSmth(_first, Direction.Left)) _first.MoveLeft();
                }
                if (!(_first is ProtectedPers))
                {
                    IntersectsEnemies(_first);
                }
                IntersectsBonuses(_first);
                _first.Draw();
                if (IsIntCoin(_first.rect))
                {
                    IntCoin(_first.rect);
                }

                Vector2 pos2 = _second.PositionOfCenter;
                if (_dInput.KeyboardUpdated)
                {
                    bool lPressed = _dInput.KeyboardState.IsPressed(Key.Left);
                    bool rPressed = _dInput.KeyboardState.IsPressed(Key.Right);
                    if (lPressed && !IsIntrSmth(_second, Direction.Left)) _second.MoveLeft();
                    if (rPressed && !IsIntrSmth(_second, Direction.Rigth)) _second.MoveRigth();

                    bool puPressed = _dInput.KeyboardState.IsPressed(Key.Up);
                    bool pdPressed = _dInput.KeyboardState.IsPressed(Key.Down);
                    if (puPressed && !IsIntrSmth(_second, Direction.Back)) _second.MoveUp();
                    if (pdPressed && !IsIntrSmth(_second, Direction.Ahead)) _second.MoveDown();
                }
                if (!(_second is ProtectedPers))
                {
                    IntersectsEnemies(_second);
                }

                IntersectsBonuses(_second);
                _second.Draw();
                if (IsIntCoin(_second.rect))
                {
                    IntCoin(_second.rect);
                }
            }
            foreach (var enemy in _enemies)
            {
                enemy.Draw();
            }
        }
        // возвращает количество собранных монет
        int GetTakenCoins()
        {
            int i = 0;
            foreach (var coin in _coins)
            {
                if (coin.IsTaken) i++;
            }
            return i;
        }

        // создание монет в рандомных местах
        void CreateCoins()
        {
            _coins = new List<Coin>();
            string _coinPath = "..\\..\\img\\CoinBonus\\";
            Random rnd = new Random();
            for (int i = 0; i < _coins_count; i++)
            {
                int x, y;
                RectangleF rect;
                do
                {
                    x = rnd.Next(0, _renderForm.Size.Width - 300);
                    y = rnd.Next(0, _renderForm.Size.Height);
                    rect = new RectangleF(x, y, 40, 40);

                } while (IsIntDoor(rect) || IsIntWall(rect) || IsIntCoin(rect));
                _coins.Add(new Coin(new Sprite(_dx2d, _coinPath, "coin"), new Vector2(x, y)));
            }

        }

        // проверяет на пересечение с монетой
        public bool IsIntCoin(RectangleF rect)
        {
            foreach (var coin in _coins)
            {
                if (rect.Intersects(coin.rect)) return true;
            }
            return false;
        }

        // при пересечении с монетой монетой становится "собранной" и больше не рисуется
        public void IntCoin(RectangleF rect)
        {
            foreach (var coin in _coins)
            {
                if (rect.Intersects(coin.rect))
                {
                    coin.TakeCoin();
                }
            }
        }

        // рандомно определяет создавать ли сейчас бонус
        bool ifCreateBonus()
        {
            Random rnd = new Random();
            int i = rnd.Next(0, 10);
            if (i == 8 || i == 4  || i == 3) return true;
            else return false;
        }

        // создание рандомного бонуса
        void CreateBonus()
        {
            Random rnd = new Random();
            int x, y;
            RectangleF rect;
            do
            {
                x = rnd.Next(0, _renderForm.Size.Width - 300);
                y = rnd.Next(0, _renderForm.Size.Height);
                rect = new RectangleF(x, y, 40, 40);

            } while (IsIntDoor(rect) || IsIntWall(rect) || IsIntCoin(rect));

            if(_bonuses == null)
                _bonuses = new List<Bonus>();
            int i = _bonuses.Count == 2 ? 6 : rnd.Next(0, (Enum.GetValues(typeof(BonusType))).Length);
            _bonuses.Add(_dx2d._bonusFactory.CreateBonus((BonusType)i, new Vector2(x, y)));
        }

        // окончание игры, определение результатов
        void GameEnd()
        {
            _gameEnded = true;
            if (_currentTime > _gameTime || !_first.IsAlive || !_second.IsAlive) _resultOfGame = "loosers.";
            if (_takenCoins == _coins_count) _resultOfGame = "winners!";
        }

        // отрисовка монет
        void DrawCoins()
        {
            foreach (var coin in _coins)
            {
                if (!coin.IsTaken)
                    coin.Draw();
            }
        }
        // отрисовка бонусов
        void DrawBonuses()
        {
            if(_bonuses != null)
            {
                foreach (var bonus in _bonuses)
                {
                    if (!bonus.IsTaken)
                        bonus.Draw();
                }
            }
            
        }
        // применение бонусов
        public void ApplyBonuses(ref Pers pers)
        {
            if (_activeBonuses == null) return;
            foreach(var bonus in _activeBonuses)
            {
                if (bonus.Value.CurrentTime < bonus.Value.Time)
                {
                    if (bonus.Key.Name == pers.Name)
                    {
                        pers = bonus.Key;
                        if (_timeHelper._isSec)
                        {
                            bonus.Value.CurrentTime++;
                            if (bonus.Value.IsUsed)
                            {
                                pers = ((SuperPers)bonus.Key).pers;
                                pers.PositionOfCenter = ((SuperPers)bonus.Key).PositionOfCenter;
                                pers.IsAlive = ((SuperPers)bonus.Key).IsAlive;
                            }
                        }
                    }
                    
                }
            }
            Dictionary<Pers, Bonus> z = new Dictionary<Pers, Bonus>();
            foreach (var bonus in _activeBonuses)
            {
                z.Add(bonus.Key, bonus.Value);
            }
            _activeBonuses.Clear();
            foreach (var bonus in z)
            {
                if (bonus.Value.Time > bonus.Value.CurrentTime)
                {
                    _activeBonuses.Add(bonus.Key, bonus.Value);
                }
            }
        }

        // проверка на пересечение с бонусами
        public void IntersectsBonuses(Pers pers)
        {
            if (_bonuses == null) return;
            foreach (Bonus bonus in _bonuses)
            {
                if (bonus.rect.Intersects(pers.rect) && !bonus.IsTaken)
                {
                    switch (bonus.bonusType)
                    {
                        case BonusType.TimePlus:
                            {
                                _currentTime -= 10;
                                bonus.TakeBonus();
                                break;
                            }
                        case BonusType.TimeMinus:
                            {
                                _currentTime += 10;
                                bonus.TakeBonus();
                                break;
                            }
                        case BonusType.SpeedPlus:
                            {
                                bonus.TakeBonus();
                                if (_activeBonuses == null)
                                {
                                    _activeBonuses = new Dictionary<Pers, Bonus>();
                                }
                                _activeBonuses.Add(((ISuperPower)bonus).SuperPower(pers), bonus);
                                break;
                            }
                        case BonusType.SpeedMinus:
                            {
                                bonus.TakeBonus();
                                if (_activeBonuses == null)
                                {
                                    _activeBonuses = new Dictionary<Pers, Bonus>();
                                }
                                _activeBonuses.Add(((ISuperPower)bonus).SuperPower(pers), bonus);
                                break;
                            }
                        case BonusType.Protection:
                            {
                                bonus.TakeBonus();
                                if (_activeBonuses == null)
                                {
                                    _activeBonuses = new Dictionary<Pers, Bonus>();
                                }
                                _activeBonuses.Add(((ISuperPower)bonus).SuperPower(pers), bonus);
                                break;
                            }
                        case BonusType.Freezing:
                            {
                                bonus.TakeBonus();
                                if (_activeBonuses == null)
                                {
                                    _activeBonuses = new Dictionary<Pers, Bonus>();
                                }
                                _activeBonuses.Add(((ISuperPower)bonus).SuperPower(pers), bonus);
                                break;
                            }
                        case BonusType.Key:
                            {
                                bonus.TakeBonus();
                                if (_activeBonuses == null)
                                {
                                    _activeBonuses = new Dictionary<Pers, Bonus>();
                                }
                                _activeBonuses.Add(((ISuperPower)bonus).SuperPower(pers), bonus);
                                break;
                            }
                    }
                }
            }
        }


        // проверка на пересечение с врагами
        public void IntersectsEnemies(Pers pers)
        {
            foreach (var enemy in _enemies)
            {
                if (enemy.rect.Intersects(pers.rect) || enemy.Purview.Intersects(pers.rect))
                {
                    pers.IsAlive = false;
                    GameEnd();
                }
            }
        }
        // проверка на пересечение со стенами
        public bool IsIntWall(RectangleF rectangle)
        {
            foreach(var wall in _walls)
            {
                if (wall.Intersects(rectangle))
                {
                    return true;
                }
            }
            return false;
        }
        // проверка на пересечение с дверьми
        public bool IsIntDoor(Pers pers)
        {
            foreach(var door in _doors)
            {
                if(pers is KeyKeeper && door.door.Intersects(pers.rect))
                {
                    Vector2 vector2 = pers.PositionOfCenter;
                    bool isAlive = pers.IsAlive;
                    door.Open();
                    pers = ((SuperPers)pers).pers;
                    pers.PositionOfCenter = vector2;
                    pers.IsAlive = isAlive;
                    return false;
                }
                if (door.door.Intersects(pers.rect) && door._isLocked )
                {
                    return true;
                }
            }
            return false;
        }
        public bool IsIntDoor(RectangleF rect)
        {
            foreach (var door in _doors)
            {
                if (door.door.Intersects(rect) && door._isLocked)
                {
                    return true;
                }
            }
            return false;
        }

        // проверка на пересечение с чем-либо
        public bool IsIntrSmth(Pers pers, Direction direction)
        {

            RectangleF rectangle = pers.rect;
            switch (direction)
            {
                case Direction.Ahead:
                    {
                        rectangle.Y+=pers.Speed;
                        if (IsIntWall(rectangle) || IsIntDoor(pers))
                        {
                            return true;
                        }
                        break;
                    }
                case Direction.Back:
                    {
                        rectangle.Y -= pers.Speed;
                        if (IsIntWall(rectangle) || IsIntDoor(pers)) 
                        {
                            return true;
                        }
                        break;
                    }
                case Direction.Left:
                    {
                        rectangle.X -= pers.Speed;
                        if (IsIntWall(rectangle) || IsIntDoor(pers))
                        {
                            return true;
                        }
                        break;
                    }
                case Direction.Rigth:
                    {
                        rectangle.X += pers.Speed;
                        if (IsIntWall(rectangle) || IsIntDoor(pers))
                        {
                            return true;
                        }
                        break;
                    }
            }
            return false;
        }

        // При ресайзинге обновляем размер области отображения и масштаб
        private void RenderForm_Resize(object sender, EventArgs e)
        {
            int width = _renderForm.ClientSize.Width;
            int height = _renderForm.ClientSize.Height;
            _dx2d.RenderTarget.Resize(new Size2(width, height));
            _clientRect.Width = _dx2d.RenderTarget.Size.Width;
            _clientRect.Height = _dx2d.RenderTarget.Size.Height;
            _scale = _clientRect.Height / _unitsPerHeight;
        }

        // ПОЕХАЛИ!!! Запуск рабочего цикла приложения (обработчик ресайзинга не забыть п )
        public void Run()
        {
            _renderForm.Resize += RenderForm_Resize;
            RenderLoop.Run(_renderForm, RenderCallback);
        }

        // Убираем за собой, удаляя неуправляемые ресурсы (здесь мамы с веником, чтобы убрала за нами нету, легко спровоцировать утечку памяти)
        public void Dispose()
        {
            _dInput.Dispose();
            _dx2d.Dispose();
            _renderForm.Dispose();
        }
    }
}