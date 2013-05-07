using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Deficit.Gameplay;
using Deficit.Images;
using Deficit.Ships;
using Microsoft.Xna.Framework;

namespace Deficit.GUI
{
    sealed class CommunicationWindow:FlickerImage
    {
        protected Ship CurrentShip;

        private const int XSpeed = 10;
        List<DrawableGameComponent> components = new List<DrawableGameComponent>();

        public CommunicationWindow(Ship ship)
        {
            CurrentShip = ship;

            X = 1280;
            Y = -20;

            Texture = ImagesManager.Get("gui-communication");
            OpacityMinimum = 0.75f;
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
        }
    }
}
