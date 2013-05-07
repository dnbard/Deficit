using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Deficit.Extentions;
using Deficit.Images;
using Deficit.core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Deficit.GUI
{
    class ParallaxComponent : DrawableGameComponent
    {
        public Image Texture { get; set; }

        protected int x;

        public int X { get; set; }
        public int Y { get; set; }
        public float Layer { get; set; }
        public int ParallaxValue { get; set; }
        public int Direction { get; set; }

        protected readonly Vector2 _viewport;
        protected readonly SpriteBatch _batch;

        public Vector2 Position
        {
            get { return new Vector2(X, Y); }
        }

        public Vector2 Size { get; set; }

        public ParallaxComponent() : base(Program.Game)
        {
            Layer = 1f;
            Direction = -1;
            ParallaxValue = 0;
            _viewport = Program.Game.Viewport;
            _batch = Program.Game.spriteBatch;
            x = 0;

            MouseHandled = false;
        }

        private string _key;
        public string TextureKey 
        { 
            set { _key = value; }
            get { return string.IsNullOrEmpty(_key) ? "full" : _key; }
        }

        public override void Draw(GameTime gameTime)
        {
            if (Texture == null) return;
            Texture.Draw(_batch, TextureKey, x, Y, Color.White, Layer);
        }

        public bool MouseHandled { get; set; }

        public EventHandler OnUpdate;
        public EventHandler OnMouseIn;
        public EventHandler OnMouseOut;
        public EventHandler OnMouseHover;
        public EventHandler OnMouseLeftClick;
        public EventHandler OnMouseRightClick;

        private bool _lastTickHover = false;
        protected bool IsHover;
        protected bool IsLeftPress;
        protected bool IsRightPress;

        public override void Update(GameTime gameTime)
        {
            if (ParallaxValue > 0)
            {
                int mX = MouseManager.X;
                float maxX = _viewport.X;

                x = (int) (mX/maxX*ParallaxValue)*Direction + X;
            }

            if (OnUpdate != null) OnUpdate(this, null);
            IsHover = VisualComponent.PointInRect(new Vector2(MouseManager.X, MouseManager.Y), new Vector2(x - Size.X * 0.5f,Y - Size.Y * 0.5f), Size);
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
        }
    }
}
