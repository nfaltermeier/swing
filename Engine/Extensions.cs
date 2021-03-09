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

        public static Rectangle WorldToScreenspace(this Rectangle r)
        {
            Vector2 offset = new Vector2(r.X, r.Y).WorldToScreenspace();
            r.X = (int)offset.X;
            r.Y = (int)offset.Y;

            return r;
        }

        public static Rectangle ScreenToWorldspace(this Rectangle r)
        {
            Vector2 offset = new Vector2(r.X, r.Y).ScreenToWorldspace();
            r.X = (int)offset.X;
            r.Y = (int)offset.Y;

            return r;
        }
    }
}
