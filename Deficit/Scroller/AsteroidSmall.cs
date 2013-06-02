using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Deficit.GUI;
using Deficit.Images;
using Deficit.Scenes;

namespace Deficit.Scroller
{
    class AsteroidSmall:BaseSpaceEntity
    {
        public float AngularSpeed = 0f;

        public AsteroidSmall()
        {
            Texture = ImagesManager.Get("space-asteroids");
            int asteroidsCount = Texture.GetNumberOfFrames("asteroid");
            Random rnd = Program.Random;

            TextureKey = "asteroid" + rnd.Next(0, asteroidsCount);

            Layer = 0.54f;
            LinearSize = 30;

            OnCollision = (self, target) =>
                {
                    SceneManager.RemoveElement(self);
                    var explosion = new StraightAnimation
                        {
                            Layer = this.Layer - 0.01f,
                            X = X,
                            Y = Y,
                            Texture = ImagesManager.Get("gfx-explosion"),
                            TextureKey = "effect",
                            PlayOnce = true
                        };
                    explosion.SetOriginToCenter();
                    ParentScene.Add(explosion);
                };

            OnTime = self =>
                {
                    self.X -= SpeedInTick;
                    if (self.X < 0 - Size.X) SceneManager.RemoveElement(self);
                };

            LinearSpeed = rnd.Next(60, 260);
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);

            Rotation += AngularSpeed;
            if (Math.Abs(Rotation) > Math.PI*2) Rotation = 0;
        }
    }
}
