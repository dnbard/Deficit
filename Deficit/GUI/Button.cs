using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Deficit.Extentions;
using Deficit.Images;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Deficit.GUI
{
    sealed class Button : VisualComponent
    {
        private string DefaultKey { get { return TextureKey; } }
        private string HoverKey { get { return TextureKey + "_hover"; } }
        private string ClickKey { get { return TextureKey + "_click"; } }

        public override void Draw(GameTime gameTime)
        {
            if (Texture == null) return;

            string currentKey = DefaultKey;
            if (IsLeftPress || IsRightPress) 
                currentKey = ClickKey;
            else if (IsHover) currentKey = HoverKey;

            Texture.Draw(Batch, currentKey, Position, 0f, Scale, Vector2.Zero, Color.White, Layer);
        }
    }
}
