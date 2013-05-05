using Deficit.Images;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Deficit.GUI
{
    class StretchBackground: DrawableGameComponent
    {
        public Image Texture { get; set; }

        private Rectangle _drawRectangle;
        private SpriteBatch batch;

        public StretchBackground() : base(Program.Game)
        {
            var viewport = Program.Game.Viewport;
            _drawRectangle = new Rectangle(0, 0, (int) viewport.X, (int) viewport.Y);
            batch = Program.Game.spriteBatch;
        }

        public override void Draw(GameTime gameTime)
        {
            if (Texture == null) return;

            Texture.Draw(batch, "full", _drawRectangle,Color.White);
        }
    }
}
