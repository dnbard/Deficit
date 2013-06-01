using System;
using Deficit.Images;
using Deficit.core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Deficit.GUI
{
    class VisualComponent: DrawableGameComponent
    {
        public virtual float X { get; set; }
        public virtual float Y { get; set; }

        private float _scale = 1f;
        public float Scale 
        {
            get { return _scale; }
            set { _scale = value; }
        }

        public float Opacity { get; set; }
        public Color Overlay { get; set; }

        public virtual Vector2 Position
        {
            get { return new Vector2(X + ParentOffsetX, Y + ParentOffsetY); }
        }

        private Vector2 _imageSize = Vector2.Zero;
        public virtual Vector2 Size
        {
            get 
            { 
                if (Texture == null) return Vector2.Zero;
                return _imageSize;
            }
            protected set { _imageSize = value; }

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

        public VisualComponent Parent { get; set; }

        protected float ParentOffsetX
        {
            get
            {
                if (Parent == null) return 0;
                return Parent.X;
            }
        }

        protected float ParentOffsetY
        {
            get
            {
                if (Parent == null) return 0;
                return Parent.Y;
            }
        }

        protected const string DefaultTextureKey = "full";
        protected string _key = DefaultTextureKey;
        public virtual string TextureKey
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
            Opacity = 1f;
            Overlay = Color.White;
        }

        public float Rotation { get; set; }

        public override void Draw(GameTime gameTime)
        {
            if (Texture == null) return;
            Texture.Draw(Batch, TextureKey, Position, Rotation, Scale, Vector2.Zero, Overlay * Opacity, Layer);
        }

        public EventHandler OnUpdate;
        public EventHandler OnMouseIn;
        public EventHandler OnMouseOut;
        public EventHandler OnMouseHover;
        public EventHandler OnMouseLeftClick;
        public EventHandler OnMouseRightClick;

        protected bool _lastTickHover = false;
        protected bool IsHover;
        protected bool IsLeftPress;
        protected bool IsRightPress;

        public override void Update(GameTime gameTime)
        {
            if (OnUpdate != null) OnUpdate(this, null);

            IsHover = PointInRect(new Vector2(MouseManager.X, MouseManager.Y), Position, Size);
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

        public static bool PointInRect(Vector2 point, Vector2 location, Vector2 size)
        {
            bool x = (point.X >= location.X) && (point.X <= location.X + size.X);
            if (!x) return false;
            bool y = (point.Y >= location.Y) && (point.Y <= location.Y + size.Y);
            return y;
        }

        public static bool PointInEllipse(Vector2 point, Vector2 location, Vector2 size)
        {
            float x = Math.Abs(location.X - point.X),
                  y = Math.Abs(location.Y - point.Y);

            bool result = x < size.X && y < size.Y;
            return result;
        }
    }
}
