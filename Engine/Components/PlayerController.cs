using Swing.Engine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using tainicom.Aether.Physics2D.Dynamics;

namespace Swing.Engine.Components
{
    class PlayerController : Component
    {
        public float Acceleration { get; set; } = 300;

        private Body body;

        public PlayerController(Actor attached) : base(attached) { }

        internal override void Start()
        {
            base.Start();
        }

        internal override void Update()
        {
            base.Update();

            Vector2 instantVel = InputManager.Direction * Acceleration * Time.DeltaTime;

            // Make stopping faster
            if (Vector2.Dot(Vector2.Normalize(instantVel), Vector2.Normalize(body.LinearVelocity)) < 0.25f)
            {
                instantVel *= 2f;
            }

            body.LinearVelocity += instantVel;
        }

        public override void FinalDestroy()
        {
            base.FinalDestroy();

            body = null;
        }
    }
}
