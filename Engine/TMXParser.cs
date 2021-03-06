using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Xml.Linq;

namespace Swing.Engine
{
    static class TMXParser
    {
        static public byte[][] ParseTilemap(string path)
        {
            XElement tilemapData = XElement.Load(path);

            XElement layer = tilemapData.Element("layer");

            byte[][] tilemap = new byte[(int)layer.Attribute("height")][];

            string data = layer.Element("data").Value;
            string[] lines = data.Split('\n').Where(l => l.Length != 0).ToArray();
            for (int i = 0; i < lines.Length; i++)
            {
                tilemap[i] = lines[i].Split(',').Where(s => s != "").Select(n => byte.Parse(n)).ToArray();
            }

            return tilemap;
        }
    }
}
