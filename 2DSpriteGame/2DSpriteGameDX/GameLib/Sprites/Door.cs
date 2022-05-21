using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2DSpriteGameDX
{
    public class Door
    {
        public RectangleF door;
        public bool _isLocked;
        public Door(RectangleF rectangle, bool isLocked)
        {
            door = rectangle;
            _isLocked = isLocked;
        }
        public Door(bool isLocked)
        {
            _isLocked = isLocked;
        }
        public void Open()
        {
            _isLocked = false;
        }
    }
}
