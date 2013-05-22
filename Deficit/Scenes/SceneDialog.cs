using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Deficit.core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Deficit.Scenes
{
    sealed class SceneDialog: Scene
    {

        private string _text;
        public string Text
        {
            get
            {
                return string.IsNullOrEmpty(_text) ? "" : _text;
            }
            set { _text = value; }
        }

        private string[] _drawStrings = null;

        private string _visibleText;
        public string VisibleText
        {
            get{ return string.IsNullOrEmpty(_visibleText) ? "" : _visibleText; }
            private set
            {
                _visibleText = value;
                _drawStrings = WordWrap(value, TextFont, 1280 - 100);
            }
        }

        public SpriteFont TextFont { get; set; }

        private int _textShow;
        public int TextShowSpeed
        {
            get { return _textShow; }
            set
            {
                if (value <= 0) return;
                
                _textShow = value;
                _textShowInteval = TimeSpan.FromSeconds(1f/value);
            }
        }

        private int TextPosition = 0;

        private TimeSpan _textShowInteval;
        private TimeSpan _lastTextUpdate = TimeSpan.FromSeconds(0);

        public Action TextDrawn;
        public Action TextDrawing;

        public SceneDialog() : base("dialog")
        {
            TextShowSpeed = 4;
            TextFont = Game.Content.Load<SpriteFont>("Fonts/defaultFont");
            TextDrawing += FasterTextDrawing;
        }

        private void FasterTextDrawing()
        {
            if (MouseManager.LeftButtonClick)
            {
                VisibleText = Text;
                TextPosition = VisibleText.Length;
            }
        }

        private Scene _nextScene;
        public Scene NextScene
        {
            get { return _nextScene; }
            set
            {
                _nextScene = value;
                if (value != null)
                    TextDrawn += NextSceneMove;
            }
        }

        private void NextSceneMove()
        {
            var cScene = SceneManager.Current;
            if (NextScene != null)
            {
                SceneManager.Current = NextScene;
                SceneManager.Delete(cScene);
            }
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);

            var time = gameTime.TotalGameTime;
            bool isTextDrawn = false;
            if (time - _lastTextUpdate > _textShowInteval)
            {
                _lastTextUpdate = time;
                if (Text.Length > TextPosition)
                {
                    TextPosition++;
                    VisibleText = Text.Substring(0, TextPosition);
                }
                else isTextDrawn = true;
            }

            if (!isTextDrawn)
            {
                if (TextDrawing != null) TextDrawing();
            }
            else if (MouseManager.LeftButtonClick) NextSceneMove();
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            SpriteBatch batch = Program.Game.spriteBatch;

            batch.Begin(SpriteSortMode.BackToFront, BlendState.NonPremultiplied);
            DrawableComponents.ForEach(element => element.Draw(gameTime));

            var i = 0; 
            if (_drawStrings != null)
                foreach (var s in _drawStrings)
                {
                    batch.DrawString(TextFont, s, new Vector2(50, 50 + 25 * i), Color.White, 0f, Vector2.Zero,1f,SpriteEffects.None, 0.1f);
                    i++;
                }
            
            batch.End();
        }

        private static string[] WordWrap(string text, SpriteFont font, int width)
        {
            if (string.IsNullOrEmpty(text)) return null;

            float currWidth = 0;
            StringBuilder str = new StringBuilder();
            List<string> result = new List<string>();

            var words = text.Split(' ');
            foreach (var word in words)
            {
                var _word = word + ' ';
                float wordWidth = font.MeasureString(_word).X;
                if (currWidth + wordWidth < width)
                {
                    str.Append(_word);
                    currWidth += wordWidth;
                }
                else
                {
                    result.Add(str.ToString());
                    str.Clear();

                    str.Append(_word);
                    currWidth = wordWidth;
                }
            }
            result.Add(str.ToString());

            return result.ToArray();
        }
    }
}
