using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Deficit.GUI;
using Deficit.Scenes;
using Microsoft.Xna.Framework;

namespace Deficit.Scroller
{
    class BaseSpaceEntity:VisualComponent
    {
        protected Vector2 Origin;
        public float LinearSize { get; set; }

        public float LinearSpeed { get; set; }

        protected float SpeedInTick { get { return LinearSpeed/60f; } }


        public Action<BaseSpaceEntity, BaseSpaceEntity> OnCollision { get; set; }
        public Action<BaseSpaceEntity> OnTime { get; set; }

        public SceneScroller ParentScene { get; set; }

        public BaseSpaceEntity()
        {
            LinearSpeed = 0f;
        }

        public override string TextureKey
        {
            get { return _key; }
            set
            {
                _key = string.IsNullOrEmpty(value) ? DefaultTextureKey : value;
                if (Texture == null)
                {
                    Size = Vector2.Zero;
                    Origin = Vector2.Zero;
                }
                else
                {
                    var rect = Texture.GetSourceRect(value);
                    Size = new Vector2(rect.Width, rect.Height);
                    Origin = new Vector2(rect.Width * 0.5f, rect.Height * 0.5f);
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (OnTime != null) OnTime(this);

            if (ParentScene == null) ParentScene = SceneManager.Get("scroller") as SceneScroller;
        }

        public override void Draw(GameTime gameTime)
        {
            if (Texture == null || !Enabled) return;
            Texture.Draw(Batch, TextureKey, Position, Rotation, Scale, Origin, Overlay * Opacity, Layer);
        }

        protected void DetectCollision(List<BaseSpaceEntity> collection)
        {
            var playerPosition = Position;

            for (int i = 0; i < collection.Count; i++)
            {
                var entity = collection[i];
                if (entity == this) continue;

                float distance = Vector2.Distance(playerPosition, entity.Position);
                float gamma = LinearSize + entity.LinearSize;

                if (distance < gamma)
                {
                    if (OnCollision != null) OnCollision(this, entity);
                    if (entity.OnCollision != null) entity.OnCollision(entity, this);
                }
            }
        }
    }
}
