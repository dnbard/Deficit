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
        public AsteroidSmall()
        {
            Texture = ImagesManager.Get("space-asteroids");
            int asteroidsCount = Texture.GetNumberOfFrames("asteroid");
            Random rnd = Program.Random;
            Alignment = EntityAlignment.Hostile;

            TextureKey = "asteroid" + rnd.Next(0, asteroidsCount);

            Layer = 0.54f;
            LinearSize = 30;

            OnDestroy = entity =>
                {
                    var explosion = new StraightAnimation
                    {
                        Layer = this.Layer - 0.01f * (float)rnd.NextDouble(),
                        X = X,
                        Y = Y,
                        Texture = ImagesManager.Get("gfx-explosion"),
                        TextureKey = "effect",
                        PlayOnce = true
                    };
                    explosion.SetOriginToCenter();
                    ParentScene.Add(explosion);

                    var debris = AsteroidDebris.Create(rnd.Next(15, 45), this);
                    ParentScene.Add(debris);
                };

            OnCollision = (self, target) =>
                {
                    target.Health -= self.Damage;
                };

            OnTime = self =>
                {
                    self.X -= SpeedInTick;
                    if (self.X < 0 - Size.X) SceneManager.RemoveElement(self);
                };

            MoveOutViewport = RemoveQuietly;

            LinearSpeed = rnd.Next(60, 260);
        }
    }
}
