using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Deficit.Events;
using Deficit.Scenes;
using Deficit.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Deficit.GUI
{
    class SelfScanOverlay:ShipOverlay
    {
        public Vector2 TextOffset { get; set; }
        public string Text { get; set; }

        private VisualText _scanResult;

        protected List<DrawableGameComponent> Components = new List<DrawableGameComponent>();
        protected SceneSelfScan ParentScene;

        private int _scaning = 0;
        public int ScaningPercentage
        {
            get { return _scaning; }
            set { if (value <= 0 && value > 100) return;
                _scaning = value;
                if (value < 100)
                {
                    _scanResult.Text = string.Format("{0}%", value);
                    _scanResult.Overlay = Color.YellowGreen;
                }
                else
                {
                    _scanResult.Text = "";
                    _scanResult.Overlay = Color.LightGreen;
                }
            }
        }

        public SelfScanOverlay(SceneSelfScan parent)
        {
            ParentScene = parent;
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

            _scanResult = new VisualText
                {
                    Text = "Unscanned",
                    Font = Game.Content.Load<SpriteFont>("Fonts/orionsmall"),
                    X = (int)TextOffset.X,
                    Y = (int)TextOffset.Y + 15,
                    Overlay = Color.MediumVioletRed
                };
            Add(_scanResult);
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

            //if (self.ParentScene.IsScaning) return;
            //self.ParentScene.IsScaning = true;

            var timedEvent = new ContinuousEventComponent(self.Components)
                {
                    FireTime = TimeSpan.FromMilliseconds(10),
                    FireCount = 100,
                    Action = o =>
                        {
                            var overlay = (SelfScanOverlay)o;
                            overlay.ScaningPercentage++;
                        },
                    EventEnd = o =>
                        {
                            var overlay = (SelfScanOverlay)o;
                            //overlay.ParentScene.IsScaning = false;
                            overlay.ScanComplete();
                        },
                    Argument = self
                };
            self.Add(timedEvent);

            self.OnMouseLeftClick -= ScanClick;
        }

        private void ScanComplete()
        {
            string name = TextureKey;
            int _x = (_scanResult.X),
                _y = (_scanResult.Y);

            ParentScene.ScansCompleted++;

            switch (name)
            {
                case "engine":
                    Add(new VisualText
                    {
                        Text = "Status: ",
                        Font = Game.Content.Load<SpriteFont>("Fonts/orionsmall"),
                        X = _x,
                        Y = _y
                    });

                    Add(new VisualText
                    {
                        Text = "Functional",
                        Font = Game.Content.Load<SpriteFont>("Fonts/orionsmall"),
                        X = _x + 80,
                        Y = _y,
                        Overlay = Color.YellowGreen
                    });

                    Add(new VisualText
                    {
                        Text = "Jump Drive Broken",
                        Font = Game.Content.Load<SpriteFont>("Fonts/orionsmall"),
                        X = _x,
                        Y = _y+18,
                        Overlay = Color.IndianRed
                    });
                    break;
                case "laboratory":
                    Add(new VisualText
                    {
                        Text = "Status: ",
                        Font = Game.Content.Load<SpriteFont>("Fonts/orionsmall"),
                        X = _x,
                        Y = _y
                    });

                    Add(new VisualText
                    {
                        Text = "Need Repair",
                        Font = Game.Content.Load<SpriteFont>("Fonts/orionsmall"),
                        X = _x + 80,
                        Y = _y,
                        Overlay = Color.IndianRed
                    });
                    break;
                case "cargo":
                    //grapple
                    Add(new VisualText
                    {
                        Text = "Contents: ",
                        Font = Game.Content.Load<SpriteFont>("Fonts/orionsmall"),
                        X = _x,
                        Y = _y
                    });

                    Add(new VisualText
                    {
                        Text = "Destroyed",
                        Font = Game.Content.Load<SpriteFont>("Fonts/orionsmall"),
                        X = _x + 105,
                        Y = _y,
                        Overlay = Color.IndianRed
                    });

                    Add(new VisualText
                    {
                        Text = "Grapple is functional",
                        Font = Game.Content.Load<SpriteFont>("Fonts/orionsmall"),
                        X = _x,
                        Y = _y + 18
                    });
                    break;
                case "weapon":
                    Add(new VisualText
                    {
                        Text = "Status: ",
                        Font = Game.Content.Load<SpriteFont>("Fonts/orionsmall"),
                        X = _x,
                        Y = _y
                    });

                    Add(new VisualText
                    {
                        Text = "Not functional",
                        Font = Game.Content.Load<SpriteFont>("Fonts/orionsmall"),
                        X = _x + 80,
                        Y = _y,
                        Overlay = Color.IndianRed
                    });
                    break;
                case "navigation":
                    Add(new VisualText
                    {
                        Text = "Status: ",
                        Font = Game.Content.Load<SpriteFont>("Fonts/orionsmall"),
                        X = _x,
                        Y = _y
                    });

                    Add(new VisualText
                    {
                        Text = "OK",
                        Font = Game.Content.Load<SpriteFont>("Fonts/orionsmall"),
                        X = _x + 80,
                        Y = _y,
                        Overlay = Color.LightGreen
                    });
                    break;
            }
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Draw(gameTime);

            for (int i = 0; i < Components.Count; i++)
            {
                var component = Components[i];
                component.Draw(gameTime);
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            for (int i = 0; i < Components.Count; i++)
            {
                var component = Components[i];
                component.Update(gameTime);
            }
        }
    }
}
