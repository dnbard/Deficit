using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Deficit.GUI;
using Deficit.Images;
using Deficit.Scenes;

namespace Deficit.Scroller
{
    class Projectile : BaseSpaceEntity
    {
        public enum ProjectileDirection {Left = -1, Right = 1}
        public ProjectileDirection Direction { get; set; }

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

        public Projectile() : base()
        {
            Texture = ImagesManager.Get("gfx-projectile");
            TextureKey = "effect";
            LinearSpeed = 400;
            Layer = 0.25f;
            Direction = ProjectileDirection.Right;

            LinearSize = 14;
            SetOriginToCenter();

            OnCollision = (self, target) =>
                {
                    SceneManager.RemoveElement(self);
                };
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            X += (int) Direction*SpeedInTick;

            base.Update(gameTime);

            DetectCollision(ParentScene.HostileObjects);

            var time = gameTime.TotalGameTime;
            if (time - LastUpdate > FrameInterval)
            {
                LastUpdate = time;
                _currFrame++;
                if (_currFrame >= MaxFrame)
                    _currFrame = 0;
            }
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (Texture == null || !Enabled) return;
            Texture.Draw(Batch, KeyFrame, Position, Rotation, Scale, Origin, Overlay * Opacity, Layer);
        }
    }
}
