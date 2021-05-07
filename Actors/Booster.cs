using System;
using System.Collections.Generic;
using System.Text;
using tainicom.Aether.Physics2D.Dynamics;
using tainicom.Aether.Physics2D.Dynamics.Contacts;
using Microsoft.Xna.Framework;
using Swing.Engine;
using Swing.Screens;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Swing.Engine.StateManagement;

namespace Swing.Actors
{
    public class Booster : BodiedActor
    {
        private Tiles boosterType;
        private Vector2 direction;
        private string spritesheetName;
        private Texture2D spritesheet;
        private int sheetTileCountX;
        private int sheetTileCountY;
        private Effect scrollEffect;

        public override RenderType RenderType => RenderType.Custom;

        public Booster(Vector2 position, int sideLength, Tiles boosterType, Vector2 direction, string spritesheetName) : base(position)
        {
            this.boosterType = boosterType;
            this.direction = Vector2.Normalize(direction);
            this.spritesheetName = spritesheetName;
            float colliderSize = (sideLength - 2) / MainGame.PhysicsScale;
            Body = MainGame.Instance.World.CreateRectangle(colliderSize, colliderSize, 20, Position / sideLength / MainGame.PhysicsScale);
            Body.OnCollision += Body_OnCollision;
            foreach (Fixture f in Body.FixtureList)
            {
                f.IsSensor = true;
            }
        }

        protected override void LoadContent(ContentManager content)
        {
            base.LoadContent(content);

            spritesheet = content.Load<Texture2D>(spritesheetName);
            sheetTileCountX = spritesheet.Width / 64;
            sheetTileCountY = spritesheet.Height / 64;
            scrollEffect = content.Load<Effect>("Scroller");
        }

        private bool Body_OnCollision(Fixture sender, Fixture other, Contact contact)
        {
            if (other.Tag is ColliderTags tag)
            {
                if (tag == ColliderTags.Player && !Player.IsBoostingDirections.Contains(boosterType))
                {
                    Vector2 normVel = other.Body.LinearVelocity;
                    normVel.Normalize();
                    // Helps prevent clipping through a booster by moving too fast the opposite direction
                    if (Vector2.Dot(direction, normVel) < -0.4f)
                    {
                        other.Body.LinearVelocity = other.Body.LinearVelocity / 2;
                    }
                    other.Body.ApplyForce(direction * 500 * other.Body.Mass * MainGame.PhysicsScale);
                    Player.IsBoostingDirections.Add(boosterType);
                }
            }

            return true;
        }

        protected override void Draw()
        {
            base.Draw();

            int spriteX = ((byte)boosterType - 1) % sheetTileCountX;
            int spriteY = ((byte)boosterType - 1) / sheetTileCountX;

            scrollEffect.Parameters["ScrollSpeed"].SetValue(0.25f);
            scrollEffect.Parameters["ScrollDirection"].SetValue(-direction);
            scrollEffect.Parameters["Time"].SetValue(Time.RealTotalTime + 10);
            scrollEffect.Parameters["TilemapDims"].SetValue(new Vector2(sheetTileCountX, sheetTileCountY));
            scrollEffect.Parameters["TilemapCoords"].SetValue(new Vector2(spriteX, spriteY));

            Screen.ScreenManager.SpriteBatch.Begin(samplerState: SamplerState.PointClamp, sortMode: SpriteSortMode.FrontToBack,
                transformMatrix: Screen.GetStandardTransformWithCamera(), rasterizerState: RasterizerState.CullClockwise, effect: scrollEffect);
            RenderSprite(Position, spritesheet, 64, 64, RenderOrder.Background);
            Screen.ScreenManager.SpriteBatch.End();
        }


    }
}
