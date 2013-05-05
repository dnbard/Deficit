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
    class ParallaxBackground : DrawableGameComponent
    {
        public Image Texture { get; set; }

        private SpriteBatch batch;

        public ParallaxBackground() : base(Program.Game)
        {
            viewport = Program.Game.Viewport;
            batch = Program.Game.spriteBatch;
        }

        private int x = 0;
        public int Y { get; set; }
        public float Layer = 1f;
        public int ParallaxValue = 0;
        private Vector2 viewport;

        public int Direction = -1;

        public int X { get; set; }

        public override void Draw(GameTime gameTime)
        {
            if (Texture == null) return;

            Texture.Draw(batch, "full", x, Y, Color.White,Layer);
        }

        public override void Update(GameTime gameTime)
        {
            if (ParallaxValue <= 0) return;

            int mX = MouseManager.X;
            float maxX = viewport.X;

            x = (int)(mX / maxX * ParallaxValue) * Direction + X;
        }
    }
}
