using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Swing.Engine;
using Swing.Components;
using Swing.Engine.StateManagement;
using System;
using System.Collections.Generic;
using System.Text;
using tainicom.Aether.Physics2D.Dynamics;
using tainicom.Aether.Physics2D.Dynamics.Contacts;
using Swing.Screens;
using Swing.Actors.ParticleSystems;

namespace Swing.Actors
{
    public class Player : BodiedActor
    {
        private Texture2D sprite;

        private PlayerController controller;

        public static HashSet<Tiles> IsBoostingDirections;

        private LandingParticleSystem landingParticleSystem;

        private PlayerSpawnParticleSystem playerSpawnParticleSystem;

        private bool isAlive = false;
        private Vector2 lastInput;

        public Player(Vector2 position, int tileSize) : base(position)
        {
            Body = MainGame.Instance.World.CreateRectangle(tileSize / MainGame.PhysicsScale, tileSize / MainGame.PhysicsScale, 50f, Position / MainGame.PhysicsScale, bodyType: BodyType.Dynamic);
            Body.FixedRotation = true;
            Body.OnCollision += Body_OnCollision;
            Body.OnSeparation += Body_OnSeparation;
            Body.LinearDamping = .5f;
            IsBoostingDirections = new HashSet<Tiles>();
            foreach (Fixture f in Body.FixtureList)
            {
                f.Tag = ColliderTags.Player;
            }
        }

        private bool Body_OnCollision(Fixture sender, Fixture other, Contact contact)
        {
            if (IsDestroyed)
                return true;

            bool ret = true;
            if (other.Tag is ColliderTags tag)
            {
                if (tag == ColliderTags.Spike)
                {
                    MainGame.Instance.Deaths++;
                    if (Screen is MainGameScreen mgs)
                    {
                        Screen.ScreenManager.QueueAddScreen(new MainGameScreen(mgs.Level));
                        Screen.ExitScreen();
                    }
                    else
                    {
                        Debug.LogError($"Player expected to be on a MainGameScreen, was on a {Screen.GetType()}");
                    }
                    Destroy();
                }
                else if (tag == ColliderTags.Wall)
                {
                    if (controller != null && !controller.IsDestroyed)
                        ret = controller.OnTouchWall(sender, other, contact);
                }
                /*else if (tag == ColliderTags.LeftBooster)
                {
                    if (controller != null && !controller.IsDestroyed)
                    {
                        this.Body.ApplyForce(Vector2.UnitX * -500 * this.Body.Mass * MainGame.PhysicsScale);
                    }
                }
                else if (tag == ColliderTags.RightBooster)
                {
                    if (controller != null && !controller.IsDestroyed)
                    {
                        this.Body.ApplyForce(Vector2.UnitX * 500 * this.Body.Mass * MainGame.PhysicsScale);
                    }
                }
                else if (tag == ColliderTags.UpBooster)
                {
                    if (controller != null && !controller.IsDestroyed)
                    {
                        this.Body.ApplyForce(Vector2.UnitY * 500 * this.Body.Mass * MainGame.PhysicsScale);
                    }
                }
                else if (tag == ColliderTags.DownBooster)
                {
                    if (controller != null && !controller.IsDestroyed)
                    {
                        this.Body.ApplyForce(Vector2.UnitY * -500 * this.Body.Mass * MainGame.PhysicsScale);
                    }
                }*/

            }

            return ret;
        }

        private void Body_OnSeparation(Fixture sender, Fixture other, Contact contact)
        {
            if (IsDestroyed)
                return;

            if (other.Tag is ColliderTags tag)
            {
                if (tag == ColliderTags.Wall)
                {
                    if (controller != null && !controller.IsDestroyed)
                        controller.OnLeaveWall(sender, other, contact);
                }

            }
        }

        protected override void Start()
        {
            base.Start();

            controller = AddComponent(new PlayerController(this));
            landingParticleSystem = new LandingParticleSystem(100);
            Screen.Instantiate(landingParticleSystem);
            playerSpawnParticleSystem = new PlayerSpawnParticleSystem(100);
            Screen.Instantiate(playerSpawnParticleSystem);
            playerSpawnParticleSystem.PlacePlayerSpawnParticle(Position);
        }

        protected override void Update()
        {
            base.Update();
            if (isAlive && controller.CheckIfLanded())
                landingParticleSystem.PlaceGroundParticle(Position - new Vector2(0, sprite.Height / 2));
            isAlive = true;
            // Store the last direction so the sprite is correct when the game is paused
            lastInput = InputManager.Direction;
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            IsBoostingDirections.Clear(); 
        }

        protected override void LoadContent(ContentManager content)
        {
            base.LoadContent(content);

            sprite = content.Load<Texture2D>("Player");
        }

        protected override void Draw()
        {
            base.Draw();

            int depth = 0;
            if (lastInput.X > 0)
                depth = 2;
            else if (lastInput.X < 0)
                depth = 1;
            RenderSpriteFromSheetCentered(Position, sprite, 64, 64, 0, 0, depth, RenderOrder.Player);
        }

        public override void FinalDestroy()
        {
            base.FinalDestroy();

            controller = null;
        }
    }
}
