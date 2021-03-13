using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Swing.Engine;
using Swing.Engine.Components;
using Swing.Engine.StateManagement;
using System;
using System.Collections.Generic;
using System.Text;
using tainicom.Aether.Physics2D.Dynamics;
using tainicom.Aether.Physics2D.Dynamics.Contacts;

namespace Swing.Actors
{
    public class Player : BodiedActor
    {
        private Texture2D sprite;
        private PlayerController controller;

        public Player(Vector2 position) : base(position)
        {
            Body = MainGame.Instance.World.CreateRectangle(64f / MainGame.PhysicsScale, 64f / MainGame.PhysicsScale, 50f, Position / MainGame.PhysicsScale, bodyType: BodyType.Dynamic);
            Body.FixedRotation = true;
            Body.OnCollision += Body_OnCollision;
            Body.OnSeparation += Body_OnSeparation;
            Body.LinearDamping = .5f;
        }

        private bool Body_OnCollision(Fixture sender, Fixture other, Contact contact)
        {
            if (other.Tag is ColliderTags tag)
            {
                if (tag == ColliderTags.Spike)
                    Destroy();
                else if (tag == ColliderTags.Wall)
                {
                    if (controller != null && !controller.IsDestroyed)
                        controller.OnTouchWall(sender, other, contact);
                }

            }

            return true;
        }

        private void Body_OnSeparation(Fixture sender, Fixture other, Contact contact)
        {
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
    }
}
