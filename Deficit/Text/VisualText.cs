using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Deficit.GUI;
using Deficit.core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Deficit.Text
{
    class VisualText: VisualComponent
    {
        protected bool RecalculateSize = true;

        private string _text = "";
        public string Text 
        { 
            get { return _text; }
            set
            {
                if (string.IsNullOrEmpty(value)) _text = "";
                else _text = value;
                RecalculateSize = true;
            }
        }

        private SpriteFont _font;
        public SpriteFont Font
        {
            get { return _font; } 
            set
            {
                _font = value;
                RecalculateSize = true;
            }
        }

        private Vector2 _size;
        public override Vector2 Size
        {
            get
            {
                if (RecalculateSize)
                {
                    _size = Font == null ? Vector2.Zero : Font.MeasureString(Text);
                    RecalculateSize = false;
                }
                return _size;
            }
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (Font == null) return;
            
            Batch.DrawString(Font,Text,Position, Overlay, 0f, Vector2.Zero, 1f,SpriteEffects.None, Layer);
        }

        public void SetDefaultFont()
        {
            Font = Game.Content.Load<SpriteFont>("Fonts/defaultFont");
        }

        public VisualText()
        {
            SetDefaultFont();
        }
    }
}
