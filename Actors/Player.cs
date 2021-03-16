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

namespace Swing.Actors
{
    public class Player : BodiedActor
    {
        private Texture2D sprite;

        private PlayerController controller;

        public Player(Vector2 position, int tileSize) : base(position)
        {
            Body = MainGame.Instance.World.CreateRectangle(tileSize / MainGame.PhysicsScale, tileSize / MainGame.PhysicsScale, 50f, Position / MainGame.PhysicsScale, bodyType: BodyType.Dynamic);
            Body.FixedRotation = true;
            Body.OnCollision += Body_OnCollision;
            Body.OnSeparation += Body_OnSeparation;
            Body.LinearDamping = .5f;
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
        }

        protected override void LoadContent(ContentManager content)
        {
            base.LoadContent(content);

            sprite = content.Load<Texture2D>("Player");
        }

        protected override void Draw()
        {
            base.Draw();

            RenderSprite(Position, sprite);
        }

        public override void FinalDestroy()
        {
            base.FinalDestroy();

            controller = null;
        }
    }
}
