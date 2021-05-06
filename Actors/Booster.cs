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

        public override RenderType RenderType => RenderType.Custom;

        public Booster(Vector2 position, int sideLength, Tiles boosterType, Vector2 direction, string spritesheetName) : base(position)
        {
            this.boosterType = boosterType;
            this.direction = direction;
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
        }

        private bool Body_OnCollision(Fixture sender, Fixture other, Contact contact)
        {
            if (other.Tag is ColliderTags tag)
            {
                if (tag == ColliderTags.Player)
                {
                    other.Body.ApplyForce(direction * 500 * other.Body.Mass * MainGame.PhysicsScale);
                }
            }

            return true;
        }

        protected override void Draw()
        {
            base.Draw();

            Screen.ScreenManager.SpriteBatch.Begin(samplerState: SamplerState.PointClamp, sortMode: SpriteSortMode.FrontToBack, transformMatrix: Screen.GetStandardTransformWithCamera(), rasterizerState: RasterizerState.CullClockwise);
            RenderSpriteFromSheetCentered(Position, spritesheet, 64, 64,
                            ((byte)boosterType - 1) % sheetTileCountX, ((byte)boosterType - 1) / sheetTileCountX, 0, RenderOrder.Background);
            Screen.ScreenManager.SpriteBatch.End();
        }


    }
}
