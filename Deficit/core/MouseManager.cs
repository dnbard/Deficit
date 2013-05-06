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

        private MouseState _mouseCurrent;
        private MouseState _mouseLast;

        public static bool MouseMoved
        {
            get 
            { 
                var current = Instance._mouseCurrent;
                var last = Instance._mouseLast;

                return current.X != last.X || current.Y != last.Y;
            }
        }

        public static int X
        {
            get { return Instance._mouseCurrent.X; }
        }

        public static int Y
        {
            get { return Instance._mouseCurrent.Y; }
        }

        public static bool LeftButtonClick
        {
            get
            {
                return Instance._mouseCurrent.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Released &&
                       Instance._mouseLast.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed;
            }
        }

        public static bool RightButtonClick
        {
            get
            {
                return Instance._mouseCurrent.RightButton == Microsoft.Xna.Framework.Input.ButtonState.Released &&
                       Instance._mouseLast.RightButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed;
            }
        }

        public static bool LeftButtonPress
        {
            get { return Instance._mouseCurrent.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed; }
        }

        public static bool RightButtonPress
        {
            get { return Instance._mouseCurrent.RightButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed; }
        }

        public override void Update(GameTime gTime)
        {
            _mouseLast = _mouseCurrent;
            _mouseCurrent = Mouse.GetState();
        }
    }
}
