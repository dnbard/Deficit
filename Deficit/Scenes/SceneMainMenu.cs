using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Deficit.GUI;
using Deficit.Gameplay;
using Deficit.Images;
using Microsoft.Xna.Framework.Graphics;

namespace Deficit.Scenes
{
    class SceneMainMenu: Scene
    {
        public SceneMainMenu() : base("mainmenu")
        {
            //Add(new GUI.StretchBackground{Texture = ImagesManager.Get("background")});
            Add(new ParallaxBackground
            {
                Texture = ImagesManager.Get("bgscroll"),
                ParallaxValue = 20,
                Layer = 1f
            });

            Add(new ParallaxBackground
            {
                Texture = ImagesManager.Get("station-default"),
                ParallaxValue = 60,
                Layer = 0.95f,
                X = 800
            });

            Add(new ParallaxBackground
            {
                Texture = ImagesManager.Get("menu0"),
                ParallaxValue = 500,
                Layer = 0.96f,
                X = 300,
                Direction = 1,
                Y = 140
            });

            /*Add(new ParallaxBackground
            {
                Texture = ImagesManager.Get("menu1"),
                ParallaxValue = 1280,
                Layer = 0.97f,
                X = -100,
                Direction = 1,
                Y = 140
            });*/

            Add(new ParallaxBackground
            {
                Texture = ImagesManager.Get("menu2"),
                ParallaxValue = 40,
                Layer = 0.97f,
                X = 440,
                Direction = -1,
                Y = 640
            });

            Add(new ParallaxBackground
            {
                Texture = ImagesManager.Get("menu3"),
                ParallaxValue = 40,
                Layer = 0.97f,
                X = 380,
                Direction = -1,
                Y = 40
            });

            Add(new ParallaxBackground
            {
                Texture = ImagesManager.Get("menu4"),
                ParallaxValue = 30,
                Layer = 0.97f,
                X = 680,
                Direction = -1,
                Y = 40
            });

            Add(new ParallaxBackground
            {
                Texture = ImagesManager.Get("menu5"),
                ParallaxValue = 25,
                Layer = 0.97f,
                X = 640,
                Direction = -1,
                Y = 250
            });

            //New Game button
            Add(new Button
                {
                    Layer = 0.5f, 
                    Texture = ImagesManager.Get("gui-menu"), 
                    TextureKey = "start", 
                    X = 50, 
                    Y = 150,
                    OnMouseLeftClick = CreateNewGame
                });
            
            //Exit button
            Add(new Button
                {
                    Layer = 0.5f, 
                    Texture = ImagesManager.Get("gui-menu"), 
                    TextureKey = "exit", 
                    X = 50, 
                    Y = 240, 
                    OnMouseLeftClick = Exit
                });
        }

        private void CreateNewGame(object sender, EventArgs args)
        {
            var mainMenu = SceneManager.Current;
            SceneManager.Current = new SceneMain();
            SceneManager.Delete(mainMenu);
        }

        private void Exit(object sender, EventArgs args)
        {
            Program.Game.Exit();
        }
    }
}
