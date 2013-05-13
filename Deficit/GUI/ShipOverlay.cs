using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Deficit.Gameplay;
using Deficit.core;
using Microsoft.Xna.Framework;

namespace Deficit.GUI
{
    class ShipOverlay : FlickerImage
    {
        private Vector2 _hoverZone = Vector2.Zero;
        public Vector2 HoverZone
        {
            get { return _hoverZone; } 
            set { _hoverZone = value; }
        }

        public float HoverWidth
        {
            set { _hoverZone = new Vector2(value, _hoverZone.Y); }
            get { return _hoverZone.X; }
        }

        public float HoverHeight
        {
            set { _hoverZone = new Vector2(_hoverZone.X, value); }
            get { return _hoverZone.Y; }
        } 

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (OnUpdate != null) OnUpdate(this, null);

            IsHover = PointInEllipse(new Vector2(MouseManager.X, MouseManager.Y),
                new Vector2(Position.X + Size.X * 0.5f, Position.Y + Size.Y * 0.5f), HoverZone);

            if (IsHover)
            {
                if (_lastTickHover)
                {
                    if (OnMouseHover != null) OnMouseHover(this, null);
                }
                else
                {
                    if (OnMouseIn != null) OnMouseIn(this, null);
                    _lastTickHover = true;
                }

                IsLeftPress = MouseManager.LeftButtonPress;
                IsRightPress = MouseManager.RightButtonPress;
                if (MouseManager.LeftButtonClick) if (OnMouseLeftClick != null) OnMouseLeftClick(this, null);
                if (MouseManager.RightButtonClick) if (OnMouseRightClick != null) OnMouseRightClick(this, null);
            }
            else if (_lastTickHover)
            {
                if (OnMouseOut != null) OnMouseOut(this, null);
                _lastTickHover = false;
                IsLeftPress = false;
                IsRightPress = false;
            }

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
