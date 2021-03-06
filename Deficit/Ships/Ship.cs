﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Deficit.GUI;
using Deficit.Gameplay;
using Deficit.Images;
using Deficit.Scenes;
using Microsoft.Xna.Framework;

namespace Deficit.Ships
{
    class Ship: ParallaxComponent
    {
        public GameFact<Race> Race = new GameFact<Race>(Races.GetRandom());

        public ShipActions CurrentAction { get; protected set; }
        private readonly SceneMain _parentScene;

        public Vector2 FlyTo { get; set; }
        public int Speed { get; protected set; }

        private string _type;
        public string ShipType
        {
            get 
            { 
                if (string.IsNullOrEmpty(_type)) return "";
                return _type;
            }
            set { _type = value; }
        }

        protected XElement ShipElement;

        private const int OverlayOffsetX = 150;
        private const int OverlayOffsetY = 260;

        public Ship(SceneMain parentScene)
        {
            if (parentScene == null) throw new ArgumentNullException("parentScene");

            ShipType = "Transport";
            ShipElement = ShipsManager.Get(ShipType);

            Texture = ImagesManager.Get("gfx-ships");
            TextureKey = "small";
            _parentScene = parentScene;
            parentScene.Ships.Add(this);

            var rect = Texture.GetSourceRect(TextureKey);
            Size = new Vector2(rect.Width, rect.Height);

            Speed = 4;

            ParallaxValue = 80;
            Layer = 0.80f;
            MouseHandled = true;
            
            CreateShip();
        }

        protected void CreateShip()
        {
            var rnd = Program.Random;

            X = rnd.Next(-450, -150);
            CurrentAction = ShipActions.FlyToContainment;
            //FlyTo = _parentScene.GetFreeSpaceInContainment();
            FlyTo = new Vector2(280, 350);
            
            Y = (int)FlyTo.Y + rnd.Next(-16, 16);
            Layer += rnd.Next(-16, 16)*0.01f;
        }

        protected float Rotation
        {
            //get { return (float)(Math.Atan2(FlyTo.Y - Y,FlyTo.X + 1 - X) - Math.PI/2); }
            get { return 0f; }
        }

        private string _name;
        public string Name
        {
            get 
            { 
                if (string.IsNullOrEmpty(_name)) return "FIX ME!";
                return _name;
            }
            set { _name = value; }
        }

        public override void Draw(GameTime gameTime)
        {
            if (Texture == null) return;

            Texture.Draw(_batch, TextureKey, new Vector2(x, Y), Rotation, 1f, new Vector2(64, 64), Color.White, Layer);
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);

            
            //_parentScene.Particles.AddTrails(x, Y);

            switch (CurrentAction)
            {
                case ShipActions.FlyToContainment:
                    //_parentScene.Particles.AddTrails(x, Y);
                    X += Speed;
                    if (X > FlyTo.X + 50)
                    {
                        CurrentAction = ShipActions.Wait;
                        _parentScene.SelectedShip = this;
                        DrawSelectCommunications();
                    }
                    break;
            }

        }

        protected void DrawSelectCommunications()
        {
            //Add(new CommunicationWindow(value));
            _parentScene.Add(new FlickerImage
            {
                Texture = ImagesManager.Get("gfx-transport"),
                TextureKey = "main",
                X = 150,
                Y = 260,
                Layer = 0.33f,
                OpacityMinimum = 0.75f
            });

            foreach (var sector in ShipElement.Element("sections").Elements())
            {
                _parentScene.Add(new ShipOverlay
                {
                    Texture = ImagesManager.Get("gfx-transport"),
                    TextureKey = sector.Attribute("texture").Value,
                    X = OverlayOffsetX + Int32.Parse(sector.Attribute("x").Value),
                    Y = OverlayOffsetY + Int32.Parse(sector.Attribute("y").Value),
                    Layer = 0.32f,
                    OpacityMinimum = 0.25f,
                    OpacityMaximum = 0.55f,
                    FirstColor = Color.White,
                    SecondColor = Color.Wheat,
                    OnMouseIn = (sender, args) =>
                    {
                        var self = sender as FlickerImage;
                        self.OpacityMinimum = 0.85f;
                        self.OpacityMaximum = 1f;
                    },
                    OnMouseOut = (sender, args) =>
                    {
                        var self = sender as FlickerImage;
                        self.OpacityMinimum = 0.25f;
                        self.OpacityMaximum = 0.55f;
                    },
                    HoverWidth = Int32.Parse(sector.Attribute("hoverx").Value),
                    HoverHeight = Int32.Parse(sector.Attribute("hovery").Value)
                });   
            }
        }
    }
}
