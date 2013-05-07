using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        private readonly Vector2 _viewport;
        protected readonly SpriteBatch _batch;

        public Vector2 Position
        {
            get { return new Vector2(X, Y); }
        }

        public Vector2 Size { get; protected set; }

        public ParallaxComponent() : base(Program.Game)
        {
            Layer = 1f;
            Direction = -1;
            ParallaxValue = 0;
            _viewport = Program.Game.Viewport;
            _batch = Program.Game.spriteBatch;
            x = 0;
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

        public override void Update(GameTime gameTime)
        {
            if (ParallaxValue <= 0) return;

            int mX = MouseManager.X;
            float maxX = _viewport.X;

            x = (int)(mX / maxX * ParallaxValue) * Direction + X;
        }
    }
}
