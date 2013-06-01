using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Deficit.GUI;
using Deficit.Images;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Deficit.Scroller.Weapons
{
    class BaseWeapon : VisualComponent
    {
        //public abstract void CreateProjectile(int x, int y);

        public BaseWeapon(BaseSpaceEntity parent)
        {
            Parent = parent;
            Layer = 0.48f;

            X = -parent.Size.X * 0.5f;
            Y = -parent.Size.Y * 0.5f;

            Texture = ImagesManager.Get("gfx-ships");
            TextureKey = "playerweapon";
        }

        public override string TextureKey
        {
            get { return base.TextureKey; }
            set
            {
                base.TextureKey = value;
                ShotKey = value + "_shot";
            }
        }

        public float xOffset { get; set; }

        public override Vector2 Position
        {
            get { return new Vector2(X + ParentOffsetX + xOffset, Y + ParentOffsetY); }
        }

        private string ShotKey;
        private string SequentialKey(GameTime gTime)
        {
            if (gTime.TotalGameTime - LastAction < ShotAnimation)
                return ShotKey;
            return TextureKey;
        }

        public override void Draw(GameTime gameTime)
        {
            if (Texture == null) return;
            Texture.Draw(Batch, SequentialKey(gameTime), Position, 0f, Scale, Vector2.Zero, Overlay * Opacity, Layer);
        }

        private TimeSpan LastAction = TimeSpan.FromSeconds(0);
        private TimeSpan ActionDelay = TimeSpan.FromMilliseconds(300);
        private TimeSpan ShotAnimation = TimeSpan.FromMilliseconds(150);

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            var keyboard = Keyboard.GetState();

            if (keyboard.IsKeyDown(Keys.Z))
            {
                var time = gameTime.TotalGameTime;

                if (time - LastAction >= ActionDelay)
                {
                    LastAction = time;
                }
            }
        }
    }
}
