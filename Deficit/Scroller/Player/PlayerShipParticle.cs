using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Deficit.GUI;
using Deficit.Images;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Deficit.Scroller.Player
{
    public enum PlayerShipParticleState {Paused, Start, Animation, Finish}

    class PlayerShipParticle : StraightAnimation
    {
        Image Start = ImagesManager.Get("gfx-shipflamein");
        Image Animation = ImagesManager.Get("gfx-shipflame");
        Image Finish = ImagesManager.Get("gfx-shipflameout");

        public PlayerShipParticleState State
        {
            get
            {
                if (!Enabled) return PlayerShipParticleState.Paused;

                if (Texture == Animation) return PlayerShipParticleState.Animation;
                else if (Texture == Start) return PlayerShipParticleState.Start;
                else if (Texture == Finish) return PlayerShipParticleState.Finish;
                
                throw (new Exception("Illegal texture in PlayerShipParticle"));
            }
        }

        public PlayerShipParticle(PlayerShip player)
        {
            Parent = player;
            TextureKey = "effect";
            FramesPerSecond = 60;
            MaxFrame = 64;
            Layer = 0.52f;
            PlayOnce = true;
            Enabled = false;
        }

        private float Rotation = (float) -Math.PI/2;

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (Texture == null || !Enabled) return;
            Texture.Draw(Batch, KeyFrame, Position, Rotation, Scale, Vector2.Zero, Overlay*Opacity, Layer);
        }

        public void StartFlames()
        {
            var state = State;
            if (state != PlayerShipParticleState.Finish && state != PlayerShipParticleState.Paused) return;
            Enabled = true;
            PlayOnce = true;
            Texture = Start;
            OnAnimationEnd = self =>
            {
                self.Enabled = true;
                self.OnAnimationEnd = null;
                self.Texture = Animation;
                self.PlayOnce = false;
                self.CurrentFrame = Program.Random.Next(0, MaxFrame);
            };
        }

        public void StopFlames()
        {
            var state = State;
            if (state == PlayerShipParticleState.Finish || state == PlayerShipParticleState.Paused) return;
            CurrentFrame = 0;
            Enabled = true;
            PlayOnce = true;
            Texture = Finish;
            OnAnimationEnd = self =>
                {
                    self.Enabled = false;
                    self.OnAnimationEnd = null;
                    self.PlayOnce = true;
                    self.CurrentFrame = 0;
                };
        }
    }
}
