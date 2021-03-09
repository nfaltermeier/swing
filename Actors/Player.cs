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

        public Player(Vector2 position) : base(position)
        {
            Body = MainGame.Instance.World.CreateRectangle(64, 64, 1, bodyType: BodyType.Dynamic);
            Body.FixedRotation = true;
            Body.OnCollision += Body_OnCollision;
        }

        private bool Body_OnCollision(Fixture sender, Fixture other, Contact contact)
        {
            if (other.Tag is ColliderTags tag && tag == ColliderTags.Spike)
                Destroy();

            return true;
        }

        protected override void Start()
        {
            base.Start();

            AddComponent(new PlayerController(this));
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
