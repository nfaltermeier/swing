using Swing.Engine;
using Swing.Engine.StateManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using tainicom.Aether.Physics2D.Dynamics;

namespace Swing.Engine.Actors
{
    class TilemapRenderer : Actor
    {
        private byte[,] tilemap;
        private string spritesheetName;
        private Texture2D spritesheet;
        private int sheetTileCountX;
        private int sheetTileCountY;
        private int tileSize;
        private List<Body> bodies;
        // offset to the top left of the top left tile
        private Vector2 topLeftOffset;

        /// <summary>
        /// Takes a position in screen space for the top left corner of the top left tile.
        /// </summary>
        /// <param name="screen"></param>
        /// <param name="position"></param>
        /// <param name="tilemap"></param>
        /// <param name="spritesheetName"></param>
        /// <param name="tileSize"></param>
        public TilemapRenderer(Vector2 position, byte[,] tilemap, string spritesheetName, int tileSize) :
            base(position)
        {
            this.tilemap = tilemap;
            this.spritesheetName = spritesheetName;
            this.tileSize = tileSize;
            topLeftOffset = new Vector2(-tilemap.GetLength(0) * tileSize / 2f, tilemap.GetLength(1) * tileSize / 2f);
            Vector2 left = GetPositionOfTile(0, 0);
            Vector2 right = GetPositionOfTile(tilemap.GetLength(0) - 1, tilemap.GetLength(1) - 1) + new Vector2(tileSize, -tileSize);
            Debug.LogClean($"x: {left.X}, y: {left.Y}, x: {right.X}, y: {right.Y}");
        }

        protected override void Start()
        {
            bodies = new List<Body>();

            float colliderSize = tileSize / MainGame.PhysicsScale;
            for (int x = 0; x < tilemap.GetLength(0); x++)
            {
                for (int y = 0; y < tilemap.GetLength(1); y++)
                {
                    if (tilemap[x, y] == (byte)Tiles.Wall || tilemap[x, y] == (byte)Tiles.UpSpike || tilemap[x, y] == (byte)Tiles.DownSpike ||
                         tilemap[x, y] == (byte)Tiles.LeftSpike || tilemap[x, y] == (byte)Tiles.RightSpike)
                    {
                        Body b = MainGame.Instance.World.CreateRectangle(colliderSize, colliderSize, 20, GetCenterOfTile(x, y) / MainGame.PhysicsScale);
                        foreach (Fixture f in b.FixtureList)
                        {
                            switch (tilemap[x, y])
                            {
                                case (byte)Tiles.Wall:
                                    f.Tag = ColliderTags.Wall;
                                    break;
                                case (byte)Tiles.UpSpike:
                                case (byte)Tiles.DownSpike:
                                case (byte)Tiles.LeftSpike:
                                case (byte)Tiles.RightSpike:
                                    f.Tag = ColliderTags.Spike;
                                    break;
                            }
                        }

                        bodies.Add(b);
                    }
                }
            }
        }

        protected override void LoadContent(ContentManager content)
        {
            base.LoadContent(content);

            spritesheet = content.Load<Texture2D>(spritesheetName);
            sheetTileCountX = spritesheet.Width / tileSize;
            sheetTileCountY = spritesheet.Height / tileSize;
        }

        protected override void Draw()
        {
            base.Draw();

            for (int x = 0; x < tilemap.GetLength(0); x++)
            {
                for (int y = 0; y < tilemap.GetLength(1); y++)
                {
                    if (tilemap[x,y] != 0 && tilemap[x,y] != (byte)Tiles.DownBooster 
                        && tilemap[x, y] != (byte)Tiles.UpBooster && tilemap[x, y] != (byte)Tiles.LeftBooster
                        && tilemap[x, y] != (byte)Tiles.RightBooster)
                    {
                        RenderSpriteFromSheet(GetPositionOfTile(x, y), spritesheet, tileSize, tileSize,
                            (tilemap[x,y] - 1) % sheetTileCountX, (tilemap[x,y] - 1) / sheetTileCountX, 0, RenderOrder.Background);
                    }
                }
            }
        }

        public override void FinalDestroy()
        {
            base.FinalDestroy();

            foreach(Body b in bodies)
            {
                MainGame.Instance.World.Remove(b);
            }

            bodies = null;
            tilemap = null;
        }

        /// <summary>
        /// Gets the top left of the tile
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public Vector2 GetPositionOfTile(int x, int y)
        {
            // Not sure why the y + 1 is needed, but it makes things line up
            return Position + topLeftOffset + new Vector2(x * tileSize, -(y + 1) * tileSize);
        }

        /// <summary>
        /// Gets the top left of the tile
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public Vector2 GetCenterOfTile(int x, int y)
        {
            return Position + topLeftOffset + new Vector2(x * tileSize + (tileSize / 2), -y * tileSize - (tileSize / 2));
        }
    }
}
