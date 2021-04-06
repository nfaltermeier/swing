using Microsoft.Xna.Framework;
using Swing;
using Swing.Actors;
using Swing.Engine;
using Swing.Engine.Actors;
using Swing.Engine.Actors.UI;
using Swing.Engine.StateManagement;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Swing.Screens
{
    class MainGameScreen : GameScreen
    {
        public int Level { get; private set; }

        private readonly int TileSize = 64;
        private Player player;
        private int xMinCameraOffset;
        private int xMaxCameraOffset;
        private int yMinCameraOffset;
        private int yMaxCameraOffset;

        public MainGameScreen(int level)
        {
            this.Level = level;
            TMXParser.ParseData data = TMXParser.ParseTilemap(GetLevelPath(level), (b => b == (byte)Tiles.Goal || b == (byte)Tiles.PlayerSpawn), TileSize);
            int levelWidth = data.width;
            int levelHeight = data.height == 1088 ? 1080 : data.height;

            xMinCameraOffset = -(levelWidth / 2) + (MainGame.Instance.DisplayWidth / 2);
            if (xMinCameraOffset > 0)
                xMinCameraOffset = 0;
            xMaxCameraOffset = (levelWidth / 2) - (MainGame.Instance.DisplayWidth / 2);
            if (xMaxCameraOffset < 0)
                xMaxCameraOffset = 0;
            yMinCameraOffset = -(levelHeight / 2) + (MainGame.Instance.DisplayHeight / 2);
            if (yMinCameraOffset > 0)
                yMinCameraOffset = 0;
            yMaxCameraOffset = (levelHeight / 2) - (MainGame.Instance.DisplayHeight / 2);
            if (yMaxCameraOffset < 0)
                yMaxCameraOffset = 0;
            TilemapRenderer renderer = Instantiate(new TilemapRenderer(Vector2.Zero, data.tiles, "Tilesheet", TileSize));

            Vector2 playerPosition = Vector2.Zero;
            foreach (Point p in data.interestingPoints)
            {
                switch (data.tiles[p.X, p.Y])
                {
                    default:
                        Debug.Log($"Unhandled interesting tile {data.tiles[p.X, p.Y]} at ({p.X}, {p.Y})");
                        break;
                    case (byte)Tiles.Goal:
                        Instantiate(new Goal(renderer.GetCenterOfTile(p.X, p.Y), TileSize));
                        break;
                    case (byte)Tiles.PlayerSpawn:
                        playerPosition = renderer.GetCenterOfTile(p.X, p.Y);
                        break;
                }
            }

            player = Instantiate(new Player(playerPosition, TileSize));
            Instantiate(new PauseListener());
            Instantiate(new TextRenderer(new Vector2(1760, 100), $"Level {Level}"));
            Instantiate(new TextRenderer(new Vector2(1760, 150), $"Deaths: {MainGame.Instance.Deaths}"));
            Instantiate(new RunTimeDisplay(new Vector2(1760, 200)));
        }

        // always draw
        public override void UpdateTransitions(bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.UpdateTransitions(otherScreenHasFocus, false);
        }

        public override void Update()
        {
            MainGame.Instance.RunTime = MainGame.Instance.RunTime.Add(TimeSpan.FromSeconds(Time.DeltaTime));
            base.Update();
            int cameraX = -MathHelper.Clamp((int)MathF.Round(player.Position.X), xMinCameraOffset, xMaxCameraOffset);
            int cameraY = -MathHelper.Clamp((int)MathF.Round(player.Position.Y), yMinCameraOffset, yMaxCameraOffset);
            CameraOffset = new Vector2(cameraX, cameraY);
        }

        private static string GetLevelPath(int index)
        {
            return $"Content/Levels/{index}.tmx";
        }

        public static bool LevelExists(int index)
        {
            return File.Exists(GetLevelPath(index));
        }
    }
}
