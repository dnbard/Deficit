using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Deficit.GUI;
using Deficit.Images;

namespace Deficit.Scenes
{
    class SceneMainMenu: Scene
    {
        public SceneMainMenu() : base("mainmenu")
        {
            Add(new GUI.StretchBackground{Texture = ImagesManager.Get("background")});

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
