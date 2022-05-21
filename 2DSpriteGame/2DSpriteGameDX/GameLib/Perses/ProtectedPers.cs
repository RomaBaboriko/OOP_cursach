using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2DSpriteGameDX.GameLib.Perses
{
    public class ProtectedPers : SuperPers
    {
        public bool Protected;
        public ProtectedPers(Pers pers) : base(pers)
        {
            Protected = true;
        }
        public override void Draw()
        {
            ActiveSprite.DrawProtection(rect);
            base.Draw();
        }
    }
}
