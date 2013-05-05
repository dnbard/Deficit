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

            Add(new Button{Layer = 0.5f, Texture = ImagesManager.Get("gui-menu"), TextureKey = "start"});
        }
    }
}
