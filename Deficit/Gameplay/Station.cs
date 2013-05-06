using Deficit.Extentions;
using Deficit.Images;
using Microsoft.Xna.Framework;

namespace Deficit.Gameplay
{
    class Station : VisualComponent  
    {
        public Station()
        {
            Texture = ImagesManager.Get("station-default");
            X = 1280 - 600;
            Y = -10;
            Layer = 0.95f;
        }
    }
}
