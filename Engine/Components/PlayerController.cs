using Swing.Engine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using tainicom.Aether.Physics2D.Dynamics;
using tainicom.Aether.Physics2D.Dynamics.Contacts;

namespace Swing.Engine.Components
{
    class PlayerController : Component
    {
        public float Acceleration { get; set; } = 3 * MainGame.PhysicsScale;
        public float JumpHangTime { get; set; } = 0.5f;

        BodiedActor bAttached;
        private Vector2? swingPoint = null;
        private Rectangle screen = new Rectangle(0, 0, 1920, 1080);
        private Dictionary<Fixture, bool> ground;

        public PlayerController(BodiedActor attached) : base(attached)
        {
            bAttached = attached;
        }

        internal override void Start()
        {
            base.Start();
            ground = new Dictionary<Fixture, bool>();
        }

        internal override void Update()
        {
            base.Update();

            Vector2 instantVel = InputManager.Direction * Acceleration * Time.DeltaTime;
            if (instantVel.Y > 0)
                instantVel.Y = 0;

            // Make stopping faster
            if (Vector2.Dot(Vector2.Normalize(instantVel), Vector2.Normalize(bAttached.Body.LinearVelocity)) < 0.25f)
            {
                instantVel *= 2f;
            }

            //Debug.Log($"Velocity: {bAttached.Body.LinearVelocity}, instantVel: {instantVel}");

            bAttached.Body.LinearVelocity += instantVel;

            /*if (InputManager.MousePressed && screen.Contains(InputManager.MouseLocation))
            {
                if (swingPoint == null)
                {
                    swingPoint = bAttached.Position + new Vector2(100, 0);
                }
                else
                {
                    swingPoint = null;
                }
            }*/

            if (InputManager.Jump && ground.Count > 0)
            {
                bAttached.Body.ApplyForce(Vector2.UnitY * 340 * bAttached.Body.Mass * MainGame.PhysicsScale);
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
                //Debug.Log($"r: {r}, a: {accel}");
                bAttached.Body.ApplyForce(am);
            }
        }

        public void OnTouchWall(Fixture sender, Fixture wall, Contact contact)
        {
            contact.GetWorldManifold(out Vector2 normal, out _);

            if (Vector2.Dot(normal, Vector2.UnitY) > 0.5f)
            {
                ground[wall] = true;
            }

            /*float xDot = Vector2.Dot(normal, Vector2.UnitX);
            if (xDot > .85f)
                rightWalls[wall] = true;
            else if (xDot < -.85f)
                leftWalls[wall] = true;*/

            if (Debug.DISPLAY_PLAYER_TOUCHING_COLLIDERS)
            {
                Debug.playerTouchingColliders[wall] = true;
            }
        }

        public void OnLeaveWall(Fixture sender, Fixture wall, Contact contact)
        {
            if (ground.ContainsKey(wall))
                ground.Remove(wall);

            if (Debug.DISPLAY_PLAYER_TOUCHING_COLLIDERS)
            {
                Debug.playerTouchingColliders.Remove(wall);
            }
        }

        internal override void Draw()
        {
            base.Draw();
            if (swingPoint is Vector2 sp)
            {
                Actor.RenderSprite(new Rectangle((int)sp.X, (int)sp.Y, 1, 1), Attached.Screen.ScreenManager.DebugPixel, Color.Red);
            }
        }

        public override void FinalDestroy()
        {
            base.FinalDestroy();

            bAttached = null;
            ground = null;

            if (Debug.DISPLAY_PLAYER_TOUCHING_COLLIDERS)
            {
                Debug.playerTouchingColliders.Clear();
            }
        }
    }
}
