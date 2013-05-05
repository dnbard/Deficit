using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Deficit.Images;

namespace Deficit.Scenes
{
    class SceneMainMenu: Scene
    {
        public SceneMainMenu() : base("mainmenu")
        {
            Add(new GUI.StretchBackground{Texture = ImagesManager.Get("background")});
        }
    }
}
