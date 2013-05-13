using System;
using Deficit.Extentions;
using Deficit.core;
using Microsoft.Xna.Framework;

namespace Deficit.GUI
{
    class FlickerImage : VisualComponent 
    {
        public FlickerImage()
        {
            FirstColor = Color.White;
            SecondColor = Color.Red;
            _tranformToColor = FirstColor;
            OpacityMinimum = 0.35f;
            OpacityMaximum = 1f;
            OpacityIncrementValue = 0.0045f;
        }

        protected bool OpacityIncrement = false;
        public float OpacityIncrementValue { get; set; }
        public float OpacityMinimum { get; set; }
        public float OpacityMaximum { get; set; }

        private Color _first;
        public Color FirstColor {
            get { return _first;  }
            set { _first = _tranformToColor = value; }
        }
        public Color SecondColor { get; set; }
        protected Color _tranformToColor;

        public Func<bool> Condition;

        public override void Draw(GameTime gameTime)
        {
            if (Condition == null || Condition())
                base.Draw(gameTime);
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);
            int mod = MouseManager.MouseMoved ? 2 : 1;

            if (OpacityIncrement)
            {
                Opacity += OpacityIncrementValue * mod;
                if (Opacity >= OpacityMaximum)
                {
                    Opacity = OpacityMaximum;
                    OpacityIncrement = false;
                }
                else if (Opacity < OpacityMinimum) Opacity = OpacityMinimum;
            }
            else
            {
                Opacity -= OpacityIncrementValue * mod;
                if (Opacity < OpacityMinimum)
                    OpacityIncrement = true;
                else if (Opacity > OpacityMaximum) Opacity = OpacityMaximum;
            }

            byte r = Overlay.R,
                 g = Overlay.G,
                 b = Overlay.B;

            int t = _tranformToColor.R - r;
            if (t > 0) r++; else if (t < 0) r--;

            t = _tranformToColor.G - g;
            if (t > 0) g++; else if (t < 0) g--;

            t = _tranformToColor.B - b;
            if (t > 0) b++; else if (t < 0) b--;

            Overlay = Color.FromNonPremultiplied(r, g, b, 255);
            if (Overlay == FirstColor) _tranformToColor = SecondColor;
            else if (Overlay == SecondColor) _tranformToColor = FirstColor;
        }
    }
}
