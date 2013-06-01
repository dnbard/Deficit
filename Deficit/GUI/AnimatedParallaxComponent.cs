using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Deficit.GUI
{
    class AnimatedParallaxComponent: ParallaxComponent
    {
        private int _currFrame = 0;
        private int _maxFrame = 32;

        private string KeyFrame
        {
            get { return TextureKey + _currFrame.ToString(); }
        }

        private int _frames;
        public int FramesPerSecond
        {
            get { return _frames; }
            set
            {
                _frames = value;
                FrameInterval = TimeSpan.FromSeconds(1f / value);
            }
        }

        private TimeSpan FrameInterval;
        private TimeSpan LastUpdate = TimeSpan.FromSeconds(0);

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            var time = gameTime.TotalGameTime;
            if (time - LastUpdate > FrameInterval)
            {
                LastUpdate = time;
                _currFrame++;
                if (_currFrame >= _maxFrame) _currFrame = 0;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            if (Texture == null) return;
            Texture.Draw(_batch, KeyFrame, new Vector2(x, Y), 0f, 1f, Vector2.Zero, Overlay * Opacity, Layer);
        }
    }
}
