using System;
using System.Collections.Generic;
using Deficit.GUI;
using Deficit.Images;
using Deficit.Scroller.Weapons;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Deficit.Scroller.Player
{
    class PlayerShip:BaseSpaceEntity
    {
        private static float _inertion = 0.85f;
        private static float MaxSpeed = 6.5f;

        public BaseWeapon Weapon;

        protected List<VisualComponent> Components = new List<VisualComponent>();
        protected PlayerShipParticle[] FlameParticles;

        public PlayerShip()
        {
            Texture = ImagesManager.Get("gfx-ships");
            TextureKey = "player";
            XSpeed = 0; 
            YSpeed = 0;
            PlaceAtCenter();
            Layer = 0.5f;
            LinearSize = 40;
            Alignment = EntityAlignment.Friendly;

            Random rnd = Program.Random;

            var GunsOffset = new Point[2];
            GunsOffset[0] = new Point(30, -40);
            GunsOffset[1] = new Point(30, 10);

            Weapon = new BaseWeapon(this){xOffset = 70,GunHoles = GunsOffset};
            Components.Add(Weapon);

            FlameParticles = new PlayerShipParticle[2];
            FlameParticles[0] = new PlayerShipParticle(this) { X = -25 - Size.X * 0.5f, Y = 44 - Size.Y * 0.5f };
            FlameParticles[1] = new PlayerShipParticle(this) { X = -25 - Size.X * 0.5f, Y = 66 - Size.Y * 0.5f };

            Components.Add(FlameParticles[0]);
            Components.Add(FlameParticles[1]);
        }

        private void StartFlames()
        {
            foreach (var flameParticle in FlameParticles)
                flameParticle.StartFlames();
        }

        private void StopFlames()
        {
            foreach (var flameParticle in FlameParticles)
                flameParticle.StopFlames();
        }

        private const float AccelerationConstant = 0.045f;
        private float AccelerationX
        {
            get
            {
                float acceleration = (MaxSpeed - XSpeed) * AccelerationConstant;
                return acceleration;
            }
        }
        private float AccelerationY
        {
            get
            {
                float acceleration = (MaxSpeed - YSpeed) * AccelerationConstant;
                return acceleration;
            }
        }


        public override float X
        {
            get { return base.X; }
            set
            {
                if (value >= Size.X * 0.5f && value <= 1280 - Size.X * 0.5f)
                    base.X = value;
            }
        }

        public override float Y
        {
            get { return base.Y; }
            set
            {
                if (value >= Size.Y * 0.5f && value <= 720 - Size.Y * 0.5f)
                    base.Y = value;
            }
        }

        private float _xspeed, _yspeed;
        protected float XSpeed
        {
            get { return _xspeed; }
            set
            {
                if (Math.Abs(value) <= MaxSpeed)
                    _xspeed = value;
            }
        }

        protected float YSpeed
        {
            get { return _yspeed; }
            set
            {
                if (Math.Abs(value) <= MaxSpeed)
                    _yspeed = value;
            }
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Draw(gameTime);

            foreach (var component in Components)
                component.Draw(gameTime);
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);

            var keyboard = Keyboard.GetState();
            if (keyboard.IsKeyDown(Keys.Right))
            {
                XSpeed += AccelerationX;
                StartFlames();
            }
            else if (keyboard.IsKeyDown(Keys.Left))
            {
                XSpeed -= AccelerationX;
                StartFlames();
            }
            else XSpeed = SpeedChange(XSpeed);

            if (keyboard.IsKeyDown(Keys.Down))
            {
                YSpeed += AccelerationY;
                StartFlames();
            }
            else if (keyboard.IsKeyDown(Keys.Up))
            {
                YSpeed -= AccelerationY;
                StartFlames();
            }
            else YSpeed = SpeedChange(YSpeed);

            X += XSpeed;
            Y += YSpeed;

            if (XSpeed == 0 && YSpeed == 0) StopFlames();

            DetectCollision(ParentScene.HostileObjects);

            foreach (var component in Components)
                component.Update(gameTime);
        }

        private static float SpeedChange(float speed)
        {
            if (speed > 0)
                speed -= _inertion;
            else if (speed < 0)
                speed += _inertion;

            if (Math.Abs(speed) < _inertion) speed = 0;

            return speed;
        }

        private void PlaceAtCenter()
        {
            X = Size.X;
            Y = (720 - Size.Y)*0.5f;
        }
    }
}
