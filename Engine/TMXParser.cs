using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Xml.Linq;
using Microsoft.Xna.Framework;

namespace Swing.Engine
{
    static class TMXParser
    {
        public class ParseData
        {
            public readonly byte[,] tiles;
            public readonly List<Point> interestingPoints;
            public readonly int height;
            public readonly int width;

            public ParseData(byte[,] tiles, List<Point> interestingPoints, int height, int width)
            {
                this.tiles = tiles;
                this.interestingPoints = interestingPoints;
                this.height = height;
                this.width = width;
            }
        }

        static public ParseData ParseTilemap(string path, Predicate<byte> interestingTester, int tileSize)
        {
            XElement tilemapData = XElement.Load(path);

            XElement layer = tilemapData.Element("layer");

            int height = (int)layer.Attribute("height");
            int width = (int)layer.Attribute("width");
            byte[][] tilemapTransposed = new byte[height][];
            byte[,] tilemap = new byte[width, height];
            List<Point> interestingPoints = new List<Point>();
            ParseData ret = new ParseData(tilemap, interestingPoints, height * tileSize, width * tileSize);

            string data = layer.Element("data").Value;
            string[] lines = data.Split('\n').Where(l => l.Length != 0).ToArray();
            for (int i = 0; i < height; i++)
            {
                tilemapTransposed[i] = lines[i].Split(',').Where(s => s != "").Select(n => byte.Parse(n)).ToArray();
            }

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    tilemap[x, y] = tilemapTransposed[y][x];
                    if (interestingTester(tilemap[x, y]))
                        interestingPoints.Add(new Point(x, y));
                }
            }

            return ret;
        }
    }
}
