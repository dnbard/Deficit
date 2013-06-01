using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Deficit.GUI;
using Deficit.Images;
using Deficit.Scroller;
using Deficit.Scroller.Player;

namespace Deficit.Scenes
{
    class SceneScroller: Scene  
    {
        public HashSet<BaseSpaceEntity> FriendlyObjects = new HashSet<BaseSpaceEntity>();
        public HashSet<BaseSpaceEntity> HostileObjects = new HashSet<BaseSpaceEntity>();

        public SceneScroller() : base("scroller")
        {
            Add(new VisualComponent
            {
                Texture = ImagesManager.Get("intro-bg3"),
                Layer = 0.99f
            });

            Add(new PlayerShip());

            Add(new AsteroidSmall()
                {
                    X = 400, Y = 250, AngularSpeed = 0.025f
                });
        }

        public override void Add(Microsoft.Xna.Framework.DrawableGameComponent element)
        {
            base.Add(element);

            var spaceObject = element as BaseSpaceEntity;
            if (spaceObject == null) return;

            if (spaceObject is PlayerShip /*AND friendly objects*/)
                FriendlyObjects.Add(spaceObject);
            else //if (spaceObject is BaseSpaceEntity)
                HostileObjects.Add(spaceObject);
        }

        public override bool Remove(Microsoft.Xna.Framework.IGameComponent element)
        {
            bool result = base.Remove(element);

            var spaceObject = element as BaseSpaceEntity;
            if (spaceObject == null) return result;

            if (FriendlyObjects.Contains(spaceObject))
            {
                FriendlyObjects.Remove(spaceObject);
                return true;
            }

            return result;
        }
    }
}
