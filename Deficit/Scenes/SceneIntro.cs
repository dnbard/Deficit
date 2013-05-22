using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Deficit.GUI;
using Deficit.Images;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Deficit.Scenes
{
    sealed class SceneIntro: Scene
    {
        private VisualComponent _background;

        public SceneIntro() : base("intro")
        {
            _background = new VisualComponent
                {
                    X = 0, Y = 0,
                    Texture = ImagesManager.Get("intro-bg0"),
                    Layer = 1
                };
            Add(_background);

            var earth = new VisualComponent
                {
                    Texture = ImagesManager.Get("intro-caption0"),
                    Layer = 0.5f,
                    Opacity = 0f,
                    OnUpdate = VisualComponentHighlight
                };
            Add(earth);

            OnTick += ShowItWasMyHome;
            OnMouseClick += CreateSubscene2;
        }

        private void CreateSubscene2(Vector2 vector2)
        {
            OnTick = null;
            OnMouseClick = null;
            Clear();

            _background = new VisualComponent
            {
                X = 0,
                Y = 0,
                Texture = ImagesManager.Get("intro-bg1"),
                Layer = 1
            };
            Add(_background);

            var caption = new VisualComponent
            {
                Texture = ImagesManager.Get("intro-caption2"),
                Layer = 0.5f,
                Opacity = 0f,
                OnUpdate = VisualComponentHighlight
            };
            Add(caption);

            Add(new IntroNext());

            OnMouseClick += CreateSubscene3;
        }

        private void CreateSubscene3(Vector2 vector2)
        {
            OnTick = null;
            OnMouseClick = null;
            Clear();

            _background = new VisualComponent
            {
                X = 0,
                Y = 0,
                Texture = ImagesManager.Get("intro-bg2"),
                Layer = 1
            };
            Add(_background);

            var caption = new VisualComponent
            {
                Texture = ImagesManager.Get("intro-caption3"),
                Layer = 0.5f,
                Opacity = 0f,
                OnUpdate = VisualComponentHighlight
            };
            Add(caption);

            Add(new IntroNext());

            OnMouseClick += CreateSubscene4;
        }

        private void CreateSubscene4(Vector2 vector2)
        {
            OnTick = null;
            OnMouseClick = null;
            Clear();

            _background = new VisualComponent
            {
                X = 0,
                Y = 0,
                Texture = ImagesManager.Get("intro-bg3"),
                Layer = 1
            };
            Add(_background);

            var caption = new VisualComponent
            {
                Texture = ImagesManager.Get("intro-caption4"),
                Layer = 0.5f,
                Opacity = 0f,
                OnUpdate = VisualComponentHighlight
            };
            Add(caption);

            Add(new IntroNext());

            OnMouseClick += CreateSubscene5;
        }

        private void CreateSubscene5(Vector2 vector2)
        {
            OnTick = null;
            OnMouseClick = null;
            Clear();

            _background = new VisualComponent
            {
                X = 0,
                Y = 0,
                Texture = ImagesManager.Get("intro-bg4"),
                Layer = 1
            };
            Add(_background);

            var caption = new VisualComponent
            {
                Texture = ImagesManager.Get("intro-caption5"),
                Layer = 0.5f,
                Opacity = 0f,
                OnUpdate = VisualComponentHighlight
            };
            Add(caption);

            Add(new IntroNext());
            OnMouseClick += StartGame;
        }

        private void StartGame(Vector2 vector2)
        {
            Program.Game.GameDate = new DateTime(2028, 12, 16);

            var intro = SceneManager.Current;
            var dialog = new SceneDialog();
            dialog.Text =
                "I've lost in the middle of nowhere. My ship is broken and i haven`t know what is wrong. \n\nNew objective: Scan ship for malfunctions.";
            //dialog.Text = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nullam feugiat justo a lorem vestibulum sit amet posuere leo sodales. In dignissim quam quis eros convallis bibendum. Sed ut leo sem, eu dapibus sem. Aenean adipiscing, risus vel rutrum condimentum, diam neque sollicitudin nunc, nec ullamcorper metus ante porttitor leo. Sed dui diam, porta quis posuere sit amet, aliquam ut purus. Phasellus elit mi, cursus et tempus egestas, congue eget elit. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia Curae; Class aptent taciti sociosqu ad litora torquent per conubia nostra, per inceptos himenaeos. Pellentesque ullamcorper, sem et consequat semper, urna felis euismod mauris, nec tincidunt elit turpis non ligula. Vestibulum ultricies mollis justo.";
            dialog.TextShowSpeed = 35;
            dialog.Add(new VisualComponent
            {
                Texture = ImagesManager.Get("intro-bg3"),
                Layer = 0.99f
            });
            dialog.NextScene = new SceneSelfScan();
            SceneManager.Current = dialog;
            SceneManager.Delete(intro);
        }

        private Action OnTick;

        private int ticks = 0;
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            ticks++;

            if (OnTick != null) OnTick();
        }

        private void ShowItWasMyHome()
        {
            if (ticks <= 80) return;

            var caption = new VisualComponent
            {
                Texture = ImagesManager.Get("intro-caption1"),
                Layer = 0.5f,
                Opacity = 0f,
                OnUpdate = VisualComponentHighlight
            };
            Add(caption);

            Add(new IntroNext());
            OnTick = null;
        }

        private void VisualComponentHighlight(object sender, EventArgs e)
        {
            var self = (VisualComponent) sender;
            if (self.Opacity >= 1f) return;
            
            self.Opacity += 0.025f;
            if (self.Opacity > 1f) self.Opacity = 1f;
        }
    }
}
