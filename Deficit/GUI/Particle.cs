using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Deficit.core;
using Microsoft.Xna.Framework;

namespace Deficit.GUI
{
    class Particle: ParallaxComponent
    {
        public int FrameCount { get; set; }
        protected int CurrentFrame = 0;

        public Particles Parent;

        public bool Repeat { get; set; }

        public override void Update(GameTime gameTime)
        {
            if (ParallaxValue >= 0)
            {
                int mX = MouseManager.X;
                float maxX = _viewport.X;

                x = (int) (mX/maxX*ParallaxValue)*Direction + X;
            }

            CurrentFrame++;
            if (CurrentFrame < FrameCount) return;
            if (Repeat) CurrentFrame = 0;
            else if (Parent != null) Parent.Remove(this);
        }

        protected string FrameKey
        {
            get { return string.Format("{0}{1}", TextureKey, CurrentFrame); }
        }

        public override void Draw(GameTime gameTime)
        {
            if (Texture == null) return;
            //Texture.Draw(_batch, TextureKey, x, Y, Color.White, Layer);
            Texture.Draw(_batch, FrameKey, x, Y, Color.White, Layer + CurrentFrame * 0.0001f);
        }
    }
}
