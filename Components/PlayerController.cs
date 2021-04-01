﻿using Swing.Engine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using tainicom.Aether.Physics2D.Dynamics;
using tainicom.Aether.Physics2D.Dynamics.Contacts;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using tainicom.Aether.Physics2D.Common;

namespace Swing.Components
{
    class PlayerController : Component
    {
        public float Acceleration { get; set; } = 3 * MainGame.PhysicsScale;
        public float JumpHangTime { get; set; } = 0.3f;
        public int CoyoteFrames { get; set; } = 4;
        public int FixedUpdateJumpCooldown { get; set; } = 3;

        BodiedActor bAttached;
        private Vector2? swingPoint = null;
        private Rectangle screen = new Rectangle(0, 0, 1920, 1080);
        private Dictionary<Fixture, bool> ground;
        private float timeSinceJump;
        private int framesSinceGrounded;
        private int jumpCooldown;

        private SoundEffect jumpSound;

        public PlayerController(BodiedActor attached) : base(attached)
        {
            bAttached = attached;
        }

        internal override void Start()
        {
            base.Start();
            ground = new Dictionary<Fixture, bool>();
            timeSinceJump = 0;
            swingPoint = null;
        }

        internal override void LoadContent(ContentManager content)
        {
            base.LoadContent(content);
            jumpSound = content.Load<SoundEffect>("Jump");
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

            if (InputManager.Jump)
            {
                if (framesSinceGrounded <= CoyoteFrames)
                {
                    framesSinceGrounded = CoyoteFrames + 1;
                    jumpCooldown = FixedUpdateJumpCooldown;
                    timeSinceJump = 0;
                    bAttached.Body.ApplyForce(Vector2.UnitY * 160 * bAttached.Body.Mass * MainGame.PhysicsScale);
                    jumpSound.Play(0.7f, 0, 0);
                }
                //bAttached.Body.ApplyForce(Vector2.UnitY * 22 * bAttached.Body.Mass * (MathF.Max(JumpHangTime - timeSinceJump, 0f) / JumpHangTime) * MainGame.PhysicsScale);
            }
        }

        internal override void FixedUpdate()
        {
            base.FixedUpdate();

            // Since jumping happens in Update and physics is stepped after fixed update, we haven't actually moved yet
            // Also it takes a couple frames for the player to actually stop touching the ground
            if (jumpCooldown == 0 && ground.Count > 0)
                framesSinceGrounded = 0;
            else
            {
                framesSinceGrounded++;
            }

            if (jumpCooldown > 0)
                jumpCooldown--;

            timeSinceJump += Time.DeltaTime;
            if (InputManager.Jump && framesSinceGrounded > CoyoteFrames && timeSinceJump <= JumpHangTime)
            {
                bAttached.Body.ApplyForce(Vector2.UnitY * 11 * bAttached.Body.Mass * MainGame.PhysicsScale);
            }

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

        public bool OnTouchWall(Fixture sender, Fixture wall, Contact contact)
        {
            bool ret = true;

            contact.GetWorldManifold(out Vector2 normal, out FixedArray2<Vector2> points);

            // If the wall was chosen to be the POV character for the contact
            bool reversed = false;
            if (sender != contact.FixtureA)
            {
                normal *= -1;
                reversed = true;
            }

            if (Vector2.Dot(normal, -Vector2.UnitY) > 0.5f)
            {
                ground[wall] = true;
            }
            else if (Vector2.Dot(normal, Vector2.UnitY) > 0.5f)
            {
                Vector2 offset = points[0] - points[1];
                if (offset.LengthSquared() < 5)
                {
                    Attached.Screen.ScreenManager.
                        AddPostPhysicsStepAction(() => sender.Body.Position += offset * (reversed ? -1 : 1));
                    ret = false;
                }
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

            return ret;
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
                Actor.RenderSpriteUI(new Rectangle((int)sp.X, (int)sp.Y, 1, 1), Attached.Screen.ScreenManager.DebugPixel, Color.Red, RenderOrder.Debug);
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
