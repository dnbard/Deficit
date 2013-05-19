using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Deficit.Images;

namespace Deficit.GUI
{
    class IntroNext:VisualComponent
    {
        public IntroNext()
        {
            Texture = ImagesManager.Get("intro-next");
            Opacity = 0f;
        }

        private bool OpacityFirstChange = true;
        private int _opacityMod = 1;
        private const float OpacityMax = 0.95f;
        private const float OpacityMin = 0.65f;
        private const float OpacityAddValue = 0.01f;

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);

            Opacity += _opacityMod*OpacityAddValue;
            if (Opacity > OpacityMax)
            {
                Opacity = OpacityMax;
                _opacityMod *= -1;
                OpacityFirstChange = false;
            }
            else if (Opacity < OpacityMin && !OpacityFirstChange)
            {
                Opacity = OpacityMin;
                _opacityMod *= -1;
                OpacityFirstChange = false;
            }
        }
    }
}
