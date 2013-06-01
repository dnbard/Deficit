using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Deficit.GUI
{
    class StraightAnimation: VisualComponent
    {
        private int _currFrame = 0;
        protected int MaxFrame = 32;

        public int CurrentFrame
        {
            get { return _currFrame; }
            set
            {
                if (value >= 0 && value <= MaxFrame)
                    _currFrame = value;
            }
        }

        protected string KeyFrame
        {
            get { return TextureKey + CurrentFrame.ToString(); }
        }

        public bool PlayOnce { get; set; }
        public Action<StraightAnimation> OnAnimationEnd { get; set; }

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
            if (Texture == null || !Enabled) return;
            Texture.Draw(Batch, KeyFrame, Position, 0f, Scale, Vector2.Zero, Overlay * Opacity, Layer);
        }

        public override void Update(GameTime gameTime)
        {
            if (!Enabled) return;
            base.Update(gameTime);

            var time = gameTime.TotalGameTime;
            if (time - LastUpdate > FrameInterval)
            {
                LastUpdate = time;
                _currFrame++;
                if (_currFrame >= MaxFrame)
                {
                    _currFrame = 0;
                    if (PlayOnce)
                    {
                        Enabled = false;
                        if (OnAnimationEnd != null) OnAnimationEnd(this);
                    }
                }
            }
        }

        public StraightAnimation()
        {
            FramesPerSecond = 24;
            PlayOnce = false;
        }
    }
}
