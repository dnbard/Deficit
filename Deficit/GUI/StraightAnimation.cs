using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Deficit.GUI
{
    class StraightAnimation: VisualComponent
    {
        private int currFrame = 0;
        private int maxFrame = 32;

        private string KeyFrame
        {
            get { return TextureKey + currFrame.ToString(); }
        }

        private int _frames;
        public int FramesPerSecond
        {
            get { return _frames; }
            set
            {
                _frames = value;
                FrameInterval = TimeSpan.FromSeconds(1f/value);
            }
        }

        private TimeSpan FrameInterval;
        private TimeSpan LastUpdate = TimeSpan.FromSeconds(0);

        public override void Draw(GameTime gameTime)
        {
            if (Texture == null) return;
            Texture.Draw(Batch, KeyFrame, Position, 0f, Scale, Vector2.Zero, Overlay * Opacity, Layer);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            var time = gameTime.TotalGameTime;
            if (time - LastUpdate > FrameInterval)
            {
                LastUpdate = time;
                currFrame++;
                if (currFrame >= maxFrame) currFrame = 0;
            }
        }

        public StraightAnimation()
        {
            FramesPerSecond = 24;
        }
    }
}
