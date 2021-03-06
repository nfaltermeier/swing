using Swing.Engine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Swing.Engine.Components
{
    class PlayerController : Component
    {
        public float Acceleration { get; set; } = 300;

        private Rigidbody2D rigidbody;

        public PlayerController(Actor attached) : base(attached) { }

        internal override void Start()
        {
            base.Start();

            rigidbody = GetComponent<Rigidbody2D>();
        }

        internal override void Update()
        {
            base.Update();

            Vector2 instantVel = InputManager.Direction * Acceleration * Time.DeltaTime;

            // Make stopping faster
            if (Vector2.Dot(Vector2.Normalize(instantVel), Vector2.Normalize(rigidbody.Velocity)) < 0.25f)
            {
                instantVel *= 2f;
            }

            rigidbody.Velocity += instantVel;
            rigidbody.IsDragEnabled = instantVel.LengthSquared() < 1f;
        }

        public override void FinalDestroy()
        {
            base.FinalDestroy();

            rigidbody = null;
        }
    }
}
