using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Deficit.Images;
using Microsoft.Xna.Framework;

namespace Deficit.Scroller
{
    class AsteroidDebris:BaseSpaceEntity
    {
        private Random _rnd;
        private float xStep, yStep;

        public static AsteroidDebris[] Create(int count, BaseSpaceEntity parent)
        {
            if (count <= 0) return null;

            var result = new AsteroidDebris[count];

            for (int i = 0; i < count; i ++)
                result[i] = new AsteroidDebris(parent);
            return result;
        }

        public AsteroidDebris(BaseSpaceEntity parent) : base()
        {
            X = parent.X;
            Y = parent.Y;

            Initialize();
        }

        public override void Initialize()
        {
            _rnd = Program.Random;
            Texture = ImagesManager.Get("space-asteroids");
            int debrisCount = Texture.GetNumberOfFrames("debris");
            TextureKey = "debris" + _rnd.Next(0, debrisCount);

            Layer = 0.67f + (float)_rnd.NextDouble()*0.01f;
            LinearSize = 5;
            AngularSpeed = (float) ((_rnd.NextDouble() - 0.5)/5);

            SetDestination();
            SetOriginToCenter();

            MoveOutViewport = RemoveQuietly;
            OnTime += entity =>
                {
                    entity.Opacity -= 0.01f;
                    if (entity.Opacity <= 0) RemoveQuietly(entity);
                };
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            X += xStep;
            Y += yStep;
        }

        private void SetDestination()
        {
            LinearSpeed = _rnd.Next(150, 800);
            var flyTo = GetOffscreenPoint();

            var distance = Vector2.Distance(Position, flyTo);
            xStep = (LinearSpeed / 60) * (Position.X - flyTo.X) / distance;
            yStep = (LinearSpeed / 60) * (Position.Y - flyTo.Y) / distance;
        }

        private Vector2 GetOffscreenPoint()
        {
            int type = _rnd.Next(0, 4);
            int x = 0, y = 0;

            switch (type)
            {
                case 0:
                    x = _rnd.Next(0, 1280);
                    y = -50;
                    break;
                case 1:
                    x = _rnd.Next(0, 1280);
                    y = 800;
                    break;
                case 2:
                    x = -50;
                    y = _rnd.Next(0, 720);
                    break;
                case 3:
                    x = 1350;
                    y = _rnd.Next(0, 720);
                    break;
            }

            return new Vector2(x, y);
        }
    }
}
