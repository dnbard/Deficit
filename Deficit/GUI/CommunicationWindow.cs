using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Deficit.Gameplay;
using Deficit.Images;
using Deficit.Ships;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Deficit.GUI
{
    sealed class CommunicationWindow:FlickerImage
    {
        protected Ship CurrentShip;

        private const int XSpeed = 10;
        List<DrawableGameComponent> components = new List<DrawableGameComponent>();

        private SpriteFont Font;

        public CommunicationWindow(Ship ship)
        {
            Font = Game.Content.Load<SpriteFont>("Fonts/orion");

            CurrentShip = ship;

            X = 1280;
            Y = -20;
            Layer = 0.6f;

            Texture = ImagesManager.Get("gui-communication");
            OpacityMinimum = 0.75f;

            components.Add(new FlickerImage
                {
                   Texture = ImagesManager.Get("gfx-graph"),
                   TextureKey = "small-default",
                   Parent = this,
                   OpacityMinimum = 0.75f,
                   OpacityIncrementValue = 0.0025f,
                   Overlay = Color.Green,
                   FirstColor = Color.Green,
                   SecondColor = Color.Yellow, 
                   X = 50,
                   Y = 200
                });
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);

            if (X > 650)
            {
                if (X - XSpeed < 650)
                    X = 650;
                else X -= XSpeed;
            }

            foreach (var component in components)
                component.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            
            foreach (var component in components)
                component.Draw(gameTime);

            Batch.DrawString(Font, CurrentShip.Name, new Vector2(X + 190, Y + 170), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.49f);
            Batch.DrawString(Font, CurrentShip.Race.Get(), new Vector2(X + 190, Y + 200), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.49f);
        }
    }
}
