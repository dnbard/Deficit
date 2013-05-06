using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Deficit.Extentions;
using Deficit.Images;
using Deficit.core;

namespace Deficit.Gameplay
{
    sealed class ContainmentSector : VisualComponent 
    {
        public ContainmentSector()
        {
            Texture = ImagesManager.Get("gui-maingrid");
            TextureKey = "full";
            X = 125;
            Y = 125;
            Layer = 0.85f;
        }

        private bool OpacityIncrement = false;
        private readonly float OpacityIncrementValue = 0.0045f;
        private readonly float OpacityMinimum = 0.35f;

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);
            int mod = MouseManager.MouseMoved ? 2 : 1;

            if (OpacityIncrement)
            {
                Opacity += OpacityIncrementValue * mod;
                if (Opacity >= 1f)
                {
                    Opacity = 1f;
                    OpacityIncrement = false;
                }
            }
            else
            {
                Opacity -= OpacityIncrementValue * mod;
                if (Opacity < OpacityMinimum)
                    OpacityIncrement = true;
            }

        }
    }
}
