using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Deficit.Extentions;
using Deficit.GUI;
using Deficit.Gameplay;
using Deficit.Images;
using Deficit.Ships;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Deficit.Scenes
{
    sealed class SceneMain: Scene
    {
        private Time Timer;

        public List<Ship> Ships = new List<Ship>();
        public Particles Particles = new Particles();

        private Ship _selected;
        public Ship SelectedShip
        {
            get { return _selected; }
            set
            {
                if (value != null)
                {
                }
                _selected = value;
            }
        }

        public SceneMain() : base("main")
        {
            Add(Particles);

            Timer = new Time();
            Timer.TimeOut += EndTurn;
            Add(Timer);

            Add(new ParallaxComponent
            {
                Texture = ImagesManager.Get("bgmain"),
                ParallaxValue = 40,
                Layer = 1f
            });

            Add(new ParallaxComponent
            {
                Texture = ImagesManager.Get("station-default"),
                ParallaxValue = 80,
                Layer = 0.95f,
                X = 1280 - 600,
                Y = -10
            });

            Add(new FlickerImage
            {
                Texture = ImagesManager.Get("gui-maingrid"),
                TextureKey = "full",
                X = 125,
                Y = 125,
                Layer = 0.85f
            });

            Add(new FlickerImage
            {
                Texture = ImagesManager.Get("gui-labels"),
                TextureKey = "quarantine",
                X = 195,
                Y = 95,
                Layer = 0.85f,
                SecondColor = Color.Red,
                FirstColor = Color.White,
                OpacityMinimum = 0.10f
            });

            Add(new FlickerImage
            {
                Texture = ImagesManager.Get("gui-labels"),
                TextureKey = "stop",
                X = 580,
                Y = 170,
                Layer = 0.85f,
                SecondColor = Color.Red,
                FirstColor = Color.White,
                OpacityMinimum = 0.15f,
                OpacityIncrementValue = 0.015f,
                Condition = new Func<bool>(() => { return SelectedShip != null; })
            });

            Add(new FlickerImage
            {
                Texture = ImagesManager.Get("gui-labels"),
                TextureKey = "enter",
                X = 580,
                Y = 400,
                Layer = 0.85f,
                SecondColor = Color.Green,
                FirstColor = Color.White,
                OpacityMinimum = 0.15f,
                OpacityIncrementValue = 0.015f,
                Condition = new Func<bool>(() => { return SelectedShip == null; })
            });

            Add(new Ship(this));
            //Add(new BriefingWindow());
        }

        private void EndTurn(object sender, EventArgs eventArgs)
        {
            
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            SpriteBatch batch = Program.Game.spriteBatch;

            batch.Begin(SpriteSortMode.BackToFront, BlendState.NonPremultiplied);
            DrawableComponents.ForEach(element => element.Draw(gameTime));
            batch.End();
            
        }

        public Vector2 GetFreeSpaceInContainment()
        {
            bool[,] place = new bool[4,3];

            for (int i = 3; i >= 0; i--)
                for (int j = 2; j >= 0; j--)
                {
                    int x = i*128 + 16 + 128;
                    int y = j*128 + 150 + 128;
                    var point = new Vector2(x, y);

                    bool inside = false;

                    foreach (var ship in Ships)
                    {
                        switch (ship.CurrentAction)
                        {
                            case ShipActions.Wait:
                                inside = VisualComponent.PointInRect(point, ship.Position, ship.Size);
                                break;
                            case ShipActions.FlyToContainment:
                                inside = VisualComponent.PointInRect(point, ship.FlyTo, ship.Size);
                                break;
                        }

                        if (!inside) continue;
                        place[i, j] = true;
                        break;
                    }
                }

            for (int i = 3; i >= 0; i--)
                for (int j = 2; j >= 0; j--)
                    if (!place[i, j]) return new Vector2(i*128 + 16 + 64 - Program.Random.Next(0, 16), j*128 + 150 + 64);
            return Vector2.Zero;
        }
    }
}
