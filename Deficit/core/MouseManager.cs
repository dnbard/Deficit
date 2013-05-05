using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Deficit.core
{
    class MouseManager : GameComponent
    {
        private static MouseManager _instance;
        public static MouseManager Instance
        {
            get { return _instance ?? (_instance = new MouseManager()); }
        }

        private MouseManager() : base(Program.Game)
        {
            
        }

        private MouseState _xnaCurrent;
        private MouseState _xnaLast;

        public static int X
        {
            get { return Instance._xnaCurrent.X; }
        }

        public static int Y
        {
            get { return Instance._xnaCurrent.Y; }
        }

        public static bool LeftButtonClick
        {
            get
            {
                return Instance._xnaCurrent.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Released &&
                       Instance._xnaLast.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed;
            }
        }

        public static bool RightButtonClick
        {
            get
            {
                return Instance._xnaCurrent.RightButton == Microsoft.Xna.Framework.Input.ButtonState.Released &&
                       Instance._xnaLast.RightButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed;
            }
        }

        public static bool LeftButtonPress
        {
            get { return Instance._xnaCurrent.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed; }
        }

        public static bool RightButtonPress
        {
            get { return Instance._xnaCurrent.RightButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed; }
        }

        public override void Update(GameTime gTime)
        {
            _xnaLast = _xnaCurrent;
            _xnaCurrent = Mouse.GetState();
        }
    }
}
