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

        BodiedActor bAttached;

        public PlayerController(BodiedActor attached) : base(attached)
        {
            bAttached = attached;
        }

        internal override void Start()
        {
            base.Start();
        }

        internal override void Update()
        {
            base.Update();

            Vector2 instantVel = InputManager.Direction * Acceleration * Time.DeltaTime;

            // Make stopping faster
            if (Vector2.Dot(Vector2.Normalize(instantVel), Vector2.Normalize(bAttached.Body.LinearVelocity)) < 0.25f)
            {
                instantVel *= 2f;
            }

            Debug.Log($"Velocity: {bAttached.Body.LinearVelocity}, instantVel: {instantVel}");

            bAttached.Body.LinearVelocity += instantVel;
        }

        public override void FinalDestroy()
        {
            base.FinalDestroy();

            bAttached = null;
        }
    }
}
