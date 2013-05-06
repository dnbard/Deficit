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
            //Add(new Station());

            Timer = new Time();
            Timer.TimeOut += EndTurn;
            Add(Timer);

            Add(new ParallaxBackground
            {
                Texture = ImagesManager.Get("bgmain"),
                ParallaxValue = 40,
                Layer = 1f
            });

            Add(new ParallaxBackground
            {
                Texture = ImagesManager.Get("station-default"),
                ParallaxValue = 40,
                Layer = 0.95f,
                X = 1280 - 600,
                Y = -10
            });

            Add(new FlickerImage
            {
                Texture = ImagesManager.Get("gui-maingrid"),
                TextureKey = "full",
                X = 125,
                Y = 125,
                Layer = 0.85f
            });

            Add(new FlickerImage
            {
                Texture = ImagesManager.Get("gui-labels"),
                TextureKey = "quarantine",
                X = 195,
                Y = 95,
                Layer = 0.85f,
                SecondColor = Color.Red,
                FirstColor = Color.White,
                OpacityMinimum = 0.10f
            });

            Add(new FlickerImage
            {
                Texture = ImagesManager.Get("gui-labels"),
                TextureKey = "stop",
                X = 580,
                Y = 170,
                Layer = 0.85f,
                SecondColor = Color.Red,
                FirstColor = Color.White,
                OpacityMinimum = 0.15f,
                OpacityIncrementValue = 0.015f
            });

            Add(new FlickerImage
            {
                Texture = ImagesManager.Get("gui-labels"),
                TextureKey = "enter",
                X = 580,
                Y = 400,
                Layer = 0.85f,
                SecondColor = Color.Green,
                FirstColor = Color.White,
                OpacityMinimum = 0.15f,
                OpacityIncrementValue = 0.015f
            });
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
