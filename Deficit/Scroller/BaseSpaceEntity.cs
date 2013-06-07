using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Deficit.GUI;
using Deficit.Scenes;
using Microsoft.Xna.Framework;

namespace Deficit.Scroller
{
    class BaseSpaceEntity : VisualComponent
    {
        protected Vector2 Origin;
        public float LinearSize { get; set; }

        public float LinearSpeed { get; set; }

        protected float SpeedInTick { get { return LinearSpeed/60f; } }

        public float AngularSpeed { get; set; }

        public Action<BaseSpaceEntity, BaseSpaceEntity> OnCollision { get; set; }
        public Action<BaseSpaceEntity> OnTime { get; set; }

        public SceneScroller ParentScene { get; set; }

        public EntityAlignment Alignment { get; protected set; }

        public BaseSpaceEntity()
        {
            LinearSpeed = 0f;
            Damage = 1;
            PositionChanged += SpaceEntityPositionChanged;

            Health = MaxHealth = 1;
        }

        protected bool isVisible;

        public Action<VisualComponent> MoveOutViewport { get; set; }
        public Action<VisualComponent> MoveInViewport { get; set; }

        private void SpaceEntityPositionChanged(VisualComponent entity)
        {
            var viewport = Program.Game.Viewport;
            var size = entity.Size;
            var visible = entity.X + size.X >= 0 && entity.X - size.X <= viewport.X &&
                        entity.Y + size.Y >= 0 && entity.Y - size.Y <= viewport.Y;

            if (!visible && isVisible && MoveOutViewport != null) //was visible, but not right now
                MoveOutViewport(this);
            else if (visible && !isVisible && MoveInViewport != null) //was out of viewport, but not right now
                MoveInViewport(this);

            isVisible = visible;
        }

        public int Damage { get; set; }

        public bool IsDamaged { get { return Health < MaxHealth; } }

        private int _health;
        public int Health
        {
            get { return _health; }
            set
            {
                if (value < _health && OnDamage != null) OnDamage(this);
                _health = value;

                if (value <= 0)
                {
                    //target is destroyed
                    if (OnDestroy != null) OnDestroy(this);
                    ParentScene.Remove(this);
                }

                if (value > MaxHealth) MaxHealth = value;
            }
        }

        public int MaxHealth { get; protected set; }

        protected Action<BaseSpaceEntity> OnDestroy { get; set; }
        protected Action<BaseSpaceEntity> OnDamage { get; set; }

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

            if (AngularSpeed != 0f)
            {
                Rotation += AngularSpeed;
                if (Math.Abs(Rotation) > Math.PI*2) Rotation = 0;
            }
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

        /// <summary>
        /// Remove object from all containers quietly without effects
        /// </summary>
        /// <param name="entity"></param>
        protected void RemoveQuietly(VisualComponent entity)
        {
            SceneManager.RemoveElement(entity);
        }
    }
}
