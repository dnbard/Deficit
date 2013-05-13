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
    sealed class BriefingWindow:FlickerImage
    {
        private const int XSpeed = 10;
        List<DrawableGameComponent> components = new List<DrawableGameComponent>();

        private SpriteFont Font;
        private string GameDate;

        public BriefingWindow()
        {
            Font = Game.Content.Load<SpriteFont>("Fonts/orionsmall");

            X = 1280;
            Y = -20;
            Layer = 0.6f;

            Texture = ImagesManager.Get("gui-briefing");
            OpacityMinimum = 1f;

            FirstColor = Color.White;
            SecondColor = Color.Wheat;

            GameDate = Program.Game.GameDate.ToShortDateString();
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

            Batch.DrawString(Font, string.Format("{0} - {1}",GameDate,"Red Square Bunker"), new Vector2(X + 215, Y + 260), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.49f);
            //Batch.DrawString(Font, CurrentShip.Race.Get(), new Vector2(X + 190, Y + 200), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.49f);
        }
    }
}
