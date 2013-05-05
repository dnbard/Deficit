using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Deficit.Images;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Deficit.Extentions
{
    class VisualComponent: DrawableGameComponent
    {
        public int X { get; set; }
        public int Y { get; set; }

        private float _scale = 1f;
        public float Scale 
        {
            get { return _scale; }
            set { _scale = value; }
        }

        protected Vector2 Position
        {
            get{ return new Vector2(X, Y);}
        }

        private Vector2 _imageSize = Vector2.Zero;
        public virtual Vector2 Size
        {
            get 
            { 
                if (Texture == null) return Vector2.Zero;
                return _imageSize;
            }
        }

        private Image _texture = null;
        public Image Texture 
        {
            get { return _texture; }
            set 
            {
                _texture = value;
                if (value != null)
                {
                    var rect = value.GetSourceRect(TextureKey);
                    _imageSize = new Vector2(rect.Width, rect.Height);
                }
                else _imageSize = Vector2.Zero;
            } 
        }
        protected SpriteBatch Batch = Program.Game.spriteBatch;

        protected const string DefaultTextureKey = "full";
        private string _key = DefaultTextureKey;
        public string TextureKey
        {
            get { return _key; }
            set
            {
                _key = string.IsNullOrEmpty(value) ? DefaultTextureKey : value;
                if (Texture == null) _imageSize = Vector2.Zero;
                else
                {
                    var rect = Texture.GetSourceRect(value);
                    _imageSize = new Vector2(rect.Width, rect.Height);
                }
            }
        }

        private float _layer = 0f;
        public float Layer
        {
            get { return _layer; }
            set
            {
                if (value < 0) _layer = 0;
                else if (value > 1) _layer = 1;
                else _layer = value;
            }
        }

        public VisualComponent() : base(Program.Game)
        {
            
        }

        public override void Draw(GameTime gameTime)
        {
            if (Texture == null) return;
            Texture.Draw(Batch, TextureKey, Position, 0f, Scale, Vector2.Zero, Color.White, Layer);
        }

        public EventHandler OnUpdate;
        public EventHandler OnMouseIn;
        public EventHandler OnMouseOut;
        public EventHandler OnMouseHover;
        public EventHandler OnMouseLeftClick;
        public EventHandler OnMouseRightClick;

        private bool lastTickHover = false;

        public override void Update(GameTime gameTime)
        {
            if (OnUpdate != null) OnUpdate(this, null);
            var mouse = Mouse.GetState();
            var mouseLast = Program.Game.LastMouseState;

            bool isHover = PointInRect(new Vector2(mouse.X, mouse.Y), Position, Size);
            if (isHover && !lastTickHover)
            {
                if (lastTickHover)
                {
                    if (OnMouseHover != null) OnMouseHover(this, null);
                }
                else if (OnMouseIn != null) OnMouseIn(this, null);

                lastTickHover = true;

                if (mouseLast.LeftButton == ButtonState.Pressed && mouse.LeftButton == ButtonState.Released)
                    if (OnMouseLeftClick != null) OnMouseLeftClick(this, null);

                if (mouseLast.RightButton == ButtonState.Pressed && mouse.RightButton == ButtonState.Released)
                    if (OnMouseRightClick != null) OnMouseRightClick(this, null);
            }
            else if (!isHover && lastTickHover)
                if (OnMouseOut != null) OnMouseOut(this, null);
        }

        public static bool PointInRect(Vector2 point, Vector2 location, Vector2 size)
        {
            bool x = point.X > location.X && point.X < location.X + size.X;
            if (!x) return false;
            bool y = point.Y > location.Y && point.Y < location.Y + size.Y;
            return y;
        }
    }

    
}
