using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Deficit.Images;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Deficit.Scenes
{
    class SceneMain: Scene
    {
        private Image texture;

        public SceneMain() : base("main")
        {
            texture = ImagesManager.Get("station-default");
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            SpriteBatch batch = Program.Game.spriteBatch;

            batch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);
            Components.ForEach(element => element.Draw(gameTime));
            texture.Draw(batch, "full", 1280 - 750, -10, Color.White);
            batch.End();
            
        }
    }
}
