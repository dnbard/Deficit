using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Deficit.Extentions;
using Deficit.GUI;
using Deficit.Gameplay;
using Deficit.Images;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Deficit.Scenes
{
    class SceneMain: Scene
    {
        private Time Timer;

        public SceneMain() : base("main")
        {
            Add(new Station());

            Timer = new Time();
            Timer.TimeOut += EndTurn;
            Add(Timer);

            Add(new ParallaxBackground
                {
                    Texture = ImagesManager.Get("bgmain"),
                    ParallaxValue = 40,
                    Layer = 1f
                });

            Add(new ContainmentSector());
        }

        private void EndTurn(object sender, EventArgs eventArgs)
        {
            
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
