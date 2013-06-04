using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Deficit.GUI;
using Deficit.Images;
using Deficit.Scroller;
using Deficit.Scroller.Player;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Deficit.Scenes
{
    class SceneScroller: Scene  
    {
        public List<BaseSpaceEntity> FriendlyObjects = new List<BaseSpaceEntity>();
        public List<BaseSpaceEntity> HostileObjects = new List<BaseSpaceEntity>();

        public SceneScroller() : base("scroller")
        {
            Add(new VisualComponent
            {
                Texture = ImagesManager.Get("intro-bg3"),
                Layer = 0.99f
            });

            Add(new PlayerShip());

            Random rnd = Program.Random;

            for (int i = 0; i < 250; i++)
            {
                Add(new AsteroidSmall()
                {
                    X = rnd.Next(1280, 12800),
                    Y = rnd.Next(0, 720),
                    AngularSpeed = ((float)rnd.NextDouble() - 0.5f) * 0.05f
                }); 
            }
        }

        public override void Add(Microsoft.Xna.Framework.DrawableGameComponent element)
        {
            base.Add(element);

            var spaceObject = element as BaseSpaceEntity;
            if (spaceObject == null) return;

            if (spaceObject is PlayerShip || spaceObject is Projectile)
                FriendlyObjects.Add(spaceObject);
            else //if (spaceObject is BaseSpaceEntity)
                HostileObjects.Add(spaceObject);
        }

        public override bool Remove(Microsoft.Xna.Framework.IGameComponent element)
        {
            bool result = base.Remove(element);

            var spaceObject = element as BaseSpaceEntity;
            if (spaceObject == null) return result;

            if (FriendlyObjects.Contains(spaceObject))
            {
                FriendlyObjects.Remove(spaceObject);
                return true;
            }

            if (HostileObjects.Contains(spaceObject))
            {
                HostileObjects.Remove(spaceObject);
                return true;
            }

            return result;
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            SpriteBatch batch = Program.Game.spriteBatch;

            batch.Begin(SpriteSortMode.BackToFront, BlendState.NonPremultiplied);
            DrawableComponents.ForEach(element => element.Draw(gameTime));
            batch.End();
        }
    }
}
