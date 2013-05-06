using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Deficit.Extentions;
using Deficit.Images;
using Deficit.core;
using Microsoft.Xna.Framework;

namespace Deficit.Gameplay
{
    sealed class FlickerImage : VisualComponent 
    {
        public FlickerImage()
        {
            FirstColor = Color.White;
            SecondColor = Color.Red;
            _tranformToColor = FirstColor;
            OpacityMinimum = 0.35f;
            OpacityIncrementValue = 0.0045f;
        }

        private bool OpacityIncrement = false;
        public float OpacityIncrementValue { get; set; }
        public float OpacityMinimum { get; set; }

        public Color FirstColor { get; set; }
        public Color SecondColor { get; set; }
        private Color _tranformToColor;

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);
            int mod = MouseManager.MouseMoved ? 2 : 1;

            if (OpacityIncrement)
            {
                Opacity += OpacityIncrementValue * mod;
                if (Opacity >= 1f)
                {
                    Opacity = 1f;
                    OpacityIncrement = false;
                }
            }
            else
            {
                Opacity -= OpacityIncrementValue * mod;
                if (Opacity < OpacityMinimum)
                    OpacityIncrement = true;
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
