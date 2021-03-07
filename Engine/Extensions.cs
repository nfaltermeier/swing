using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Swing.Engine
{
    static class Extensions
    {
        
        public static Vector2 ScreenToWorldspace(this Vector2 v)
        {
            return new Vector2(v.X - (MainGame.Instance.DisplayWidth / 2), (MainGame.Instance.DisplayHeight / 2) - v.Y);
        }

        public static Vector2 WorldToScreenspace(this Vector2 v)
        {
            return new Vector2(v.X + (MainGame.Instance.DisplayWidth / 2), (MainGame.Instance.DisplayHeight / 2) - v.Y);
        }

        public static Vector2 SetLength(this Vector2 v, float length)
        {
            v = Vector2.Normalize(v);
            return v *= length;
        }
    }
}
