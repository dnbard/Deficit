using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Deficit.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Deficit.GUI
{
    internal enum ScanState
    { 
        Unscanned, InScan, Scanned
    }

    class SelfScanOverlay:ShipOverlay
    {
        public Vector2 TextOffset { get; set; }
        public string Text { get; set; }

        private ScanState State;

        private VisualText ScanResult;

        protected List<DrawableGameComponent> Components = new List<DrawableGameComponent>();

        public SelfScanOverlay()
        {
            OnMouseLeftClick += ScanClick;
        }

        public override void Initialize()
        {
            Add(new VisualText
            {
                Text = Text,
                Font = Game.Content.Load<SpriteFont>("Fonts/orionsmall"),
                X = (int)TextOffset.X,
                Y = (int)TextOffset.Y
            });

            ScanResult = new VisualText
                {
                    Text = "Unscanned",
                    Font = Game.Content.Load<SpriteFont>("Fonts/orionsmall"),
                    X = (int)TextOffset.X,
                    Y = (int)TextOffset.Y + 15,
                    Overlay = Color.ForestGreen
                };
            Add(ScanResult);
        }

        protected void Add(DrawableGameComponent component)
        {
            if (!Components.Contains(component))
                Components.Add(component);
        }

        private static void ScanClick(object sender, EventArgs eventArgs)
        {
            var self = (SelfScanOverlay) sender;
            if (self == null) return;

            self.OnMouseLeftClick -= ScanClick;

        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Draw(gameTime);

            foreach (var component in Components)
                component.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            foreach (var component in Components)
                component.Update(gameTime);
        }
    }
}
