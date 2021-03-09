using Microsoft.Xna.Framework;
using Swing;
using Swing.Actors;
using Swing.Engine;
using Swing.Engine.Actors;
using Swing.Engine.StateManagement;
using System;
using System.Collections.Generic;
using System.Text;

namespace Swing.Screens
{
    class MainGameScreen : GameScreen
    {
        public MainGameScreen()
        {
            TMXParser.ParseData data = TMXParser.ParseTilemap("Content/Levels/1.tmx", (b => b == (byte)Tiles.Goal || b == (byte)Tiles.PlayerSpawn));
            TilemapRenderer renderer = Instantiate(new TilemapRenderer(Vector2.Zero, data.tiles, "Tilesheet", 64)) as TilemapRenderer;

            Vector2 playerPosition = Vector2.Zero;
            foreach (Point p in data.interestingPoints)
            {
                switch (data.tiles[p.X, p.Y])
                {
                    default:
                        Debug.Log($"Unhandled interesting tile {data.tiles[p.X, p.Y]} at ({p.X}, {p.Y})");
                        break;
                    case (byte)Tiles.Goal:
                        break;
                    case (byte)Tiles.PlayerSpawn:
                        playerPosition = renderer.GetPositionOfTile(p.X, p.Y);
                        break;
                }
            }

            Instantiate(new Player(playerPosition));
        }
    }
}
