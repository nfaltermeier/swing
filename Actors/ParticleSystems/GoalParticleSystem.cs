using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Swing.Engine.Actors.Particles;
using Swing.Engine;

namespace Swing.Actors.ParticleSystems
{
    public class GoalParticleSystem : ParticleSystem
    {
        private Vector2 emitPosition;

        public GoalParticleSystem(int maxParticles, Vector2 position) : base(Vector2.Zero, maxParticles)
        {
            emitPosition = position;
        }

        protected override void InitializeConstants()
        {
            textureFilename = "Engine/1x1WhitePixel";
            minNumParticles = 1;
            maxNumParticles = 1;

        }

        protected override void InitializeParticle(ref Particle p, Vector2 where)
        {
            base.InitializeParticle(ref p, where);

            var velocity = Utilities.random.NextDirection() * Utilities.random.NextFloat(10, 25);

            var lifetime = Utilities.random.NextFloat(3, 6);

            var acceleration = Utilities.random.NextDirection() * 5;

            var rotation = Utilities.random.NextFloat(0, MathHelper.TwoPi);

            var angularVelocity = Utilities.random.NextFloat(0, MathHelper.PiOver4);

            var scale = Utilities.random.NextFloat(3, 5);

            Color color = new Color(76, 255, 0);

            p.Initialize(where, velocity, acceleration, color, lifetime: lifetime, rotation: rotation, angularVelocity: angularVelocity, scale: scale);
        }

        protected override void Update()
        {
            base.Update();
            AddParticles(emitPosition + Utilities.random.NextDirection() * Utilities.random.NextFloat(16, 64));
        }

    }
}
