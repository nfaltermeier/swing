using Swing.Engine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Swing.Engine.Components
{
    internal class Rigidbody2D : Component
    {
        public Vector2 Velocity { get; set; } = Vector2.Zero;
        public Vector2 Acceleration { get; set; } = Vector2.Zero;
        public float MaxVelocity { get; set; } = float.PositiveInfinity;
        public float MaxAcceleration { get; set; } = float.PositiveInfinity;
        public bool IsDragEnabled { get; set; } = true;
        public float Drag { get; set; } = 0;

        public Rigidbody2D(Actor attached) : base(attached) { }

        internal override void FixedUpdate()
        {
            base.Update();

            float deltaTime = Time.DeltaTime;

            if (!float.IsInfinity(MaxAcceleration) && Acceleration.LengthSquared() > (MaxAcceleration * MaxAcceleration))
            {
                Acceleration = Acceleration.SetLength(MaxAcceleration);
            }
            Velocity += Acceleration * deltaTime;

            if (!float.IsInfinity(MaxVelocity) && Velocity.LengthSquared() > (MaxVelocity * MaxVelocity))
            {
                Velocity = Velocity.SetLength(MaxVelocity);
            }
            Attached.Position += Velocity * deltaTime;

            float velocityMag = Velocity.LengthSquared();
            if (IsDragEnabled && Drag != 0 && velocityMag != 0)
            {
                Vector2 newVelocity = Velocity;

                // Prevent from dragging more than the actual velocity
                newVelocity.X += MathF.Min((newVelocity.X * newVelocity.X) / velocityMag * Drag * deltaTime, MathF.Abs(newVelocity.X)) * (float.IsNegative(Velocity.X) ? 1 : -1);
                newVelocity.Y += MathF.Min((newVelocity.Y * newVelocity.Y) / velocityMag * Drag * deltaTime, MathF.Abs(newVelocity.Y)) * (float.IsNegative(Velocity.Y) ? 1 : -1);

                Velocity = newVelocity;
            }

        }
    }
}
