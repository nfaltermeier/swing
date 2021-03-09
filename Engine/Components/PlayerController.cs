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
        private Vector2? swingPoint = null;
        private Rectangle screen = new Rectangle(0, 0, 1920, 1080);

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

            if (InputManager.MouseClicked && screen.Contains(InputManager.MouseLocation))
            {
                if (swingPoint == null)
                {
                    swingPoint = bAttached.Position + new Vector2(100, 0);
                }
                else
                {
                    swingPoint = null;
                }
            }
        }

        internal override void FixedUpdate()
        {
            base.FixedUpdate();

            if (swingPoint is Vector2 sp)
            {
                Vector2 accel = sp - bAttached.Position;
                float r = accel.Length();
                accel.Normalize();

                if (r > 100)
                {
                    bAttached.Position = sp - accel * 100;
                    bAttached.Body.LinearVelocity += accel;
                }

                //if (r > 101 || r < 99)
                    //accel *= MathF.Pow(r - 100, 7);

                float velsq = bAttached.Body.LinearVelocity.LengthSquared();
                accel *= (velsq / 100f);
                Vector2 am = accel * bAttached.Body.Mass;
                Debug.Log($"r: {r}, a: {accel}");
                bAttached.Body.ApplyForce(am);
            }
        }


        internal override void Draw()
        {
            base.Draw();
            if (swingPoint is Vector2 sp)
            {
                Attached.RenderSprite(new Rectangle((int)sp.X, (int)sp.Y, 1, 1), Attached.Screen.ScreenManager.DebugPixel, Color.Red);
            }
        }

        public override void FinalDestroy()
        {
            base.FinalDestroy();

            bAttached = null;
        }
    }
}
