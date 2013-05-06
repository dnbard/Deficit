using System;
using System.Globalization;
using Deficit.Images;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Deficit.Gameplay
{
    class Time: DrawableGameComponent
    {
        private const int DefaultTurnLength = 90;
        readonly private TimeSpan _oneSecond = TimeSpan.FromSeconds(1);

        public int TimeLeft { get; set; }
        private TimeSpan _lastUpdate = TimeSpan.FromSeconds(0);

        private readonly SpriteFont _font;
        private readonly SpriteFont _fontBig;
        private readonly SpriteBatch _batch;

        private Image texture;

        public Time() : base(Program.Game)
        {
            TimeLeft = DefaultTurnLength;
            _font = Game.Content.Load<SpriteFont>("Fonts/defaultFont");
            _fontBig = Game.Content.Load<SpriteFont>("Fonts/arialBig");
            _batch = Program.Game.spriteBatch;

            texture = ImagesManager.Get("gui-labels");
        }

        public void ResetTimer()
        {
            TimeLeft = DefaultTurnLength;
        }

        public void Stop()
        {
            Enabled = false;
        }

        public void Start()
        {
            Enabled = true;
        }

        public event EventHandler TimeOut;

        public override void Update(GameTime gameTime)
        {
            if (!Enabled) return;

            if (gameTime.TotalGameTime - _lastUpdate > _oneSecond)
            {
                _lastUpdate = gameTime.TotalGameTime;
                TimeLeft--;
                if (TimeLeft <= 0)
                {
                    Stop();
                    if (TimeOut != null) TimeOut(this, null);
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            if (Enabled)
            {
                float mod = 1 - (float)TimeLeft/DefaultTurnLength;

                Color clr = Color.FromNonPremultiplied(154 + (int) (100*mod), 205 - (int) (205*mod),
                                                       50 - (int) (50*mod), 255);

                _batch.DrawString(_font, "TIME", new Vector2(1100, 10), Color.White, 0f, Vector2.Zero, 1f,
                                 SpriteEffects.None, 0.5f);
                _batch.DrawString(_font, "LEFT", new Vector2(1100, 40), Color.White, 0f, Vector2.Zero, 1f,
                                 SpriteEffects.None, 0.5f);
                _batch.DrawString(_fontBig, TimeLeft.ToString(CultureInfo.InvariantCulture), new Vector2(1190, 5), clr, 0f, Vector2.Zero, 1f,
                                 SpriteEffects.None, 0.5f);
            }
            else
            {
                //_batch.DrawString(_font, "|| PAUSED", new Vector2(1100, 10), Color.White, 0f, Vector2.Zero, 1f,
                //                 SpriteEffects.None, 0.5f);
                texture.Draw(_batch, "pause", new Vector2(1100, 10), Color.White,0.5f);
            }
        }
    }
}
