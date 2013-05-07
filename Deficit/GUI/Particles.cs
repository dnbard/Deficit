using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Deficit.Images;
using Microsoft.Xna.Framework;

namespace Deficit.GUI
{
    class Particles : DrawableGameComponent
    {
        private readonly List<Particle> particles = new List<Particle>(600);

        public Particles() : base(Program.Game)
        {
            
        }

        public void Remove(Particle p)
        {
            if (particles.Contains(p)) particles.Remove(p);
        }

        public override void Update(GameTime gameTime)
        {
            int i = 0;
            while (i < particles.Count)
            {
                particles[i].Update(gameTime);
                i++;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            foreach (var particle in particles)
                particle.Draw(gameTime);
        }

        public void AddTrails(int x, int y)
        {
            var particle = new Particle
                {
                    X = x,
                    Y = y,
                    ParallaxValue = 0,
                    Texture = ImagesManager.Get("gfx-hit"),
                    TextureKey = "firespark",
                    Layer = 0.75f
                };
            particles.Add(particle);
        }
    }
}
