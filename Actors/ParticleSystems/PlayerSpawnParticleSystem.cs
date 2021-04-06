using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Swing.Engine.Actors.Particles;
using Swing.Engine;

namespace Swing.Actors.ParticleSystems
{
    public class PlayerSpawnParticleSystem : ParticleSystem
    {
        public PlayerSpawnParticleSystem(int maxParticles) : base(Vector2.Zero, maxParticles)
        {

        }

        protected override void InitializeConstants()
        {
            textureFilename = "Engine/1x1WhitePixel";
            minNumParticles = 20;
            maxNumParticles = 25;
        }

        protected override void InitializeParticle(ref Particle p, Vector2 where)
        {
            base.InitializeParticle(ref p, where);

            var velocity = Utilities.random.NextDirection() * Utilities.random.NextFloat(40, 200);

            var lifetime = Utilities.random.NextFloat(0.5f, 1.0f);

            var acceleration = -velocity / lifetime;

            var rotation = Utilities.random.NextFloat(0, MathHelper.TwoPi);

            var angularVelocity = Utilities.random.NextFloat(-MathHelper.PiOver4, MathHelper.PiOver4);

            var scale = Utilities.random.NextFloat(3, 5);

            p.Initialize(where, velocity, acceleration, Color.Blue, lifetime: lifetime, rotation: rotation, angularVelocity: angularVelocity, scale: scale);
        }
        protected override void UpdateParticle(ref Particle particle, float dt)
        {
            base.UpdateParticle(ref particle, dt);

            float normalizedLifetime = particle.TimeSinceStart / particle.Lifetime;

            // I don't know why this works, but it adds a sparkling effect (by making the particles get bigger at the end)
            particle.Scale -= .25f * normalizedLifetime;
        }

        public void PlacePlayerSpawnParticle(Vector2 where)
        {
            AddParticles(where);
        }
    }
}
