using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Swing.Engine
{
    static class Utilities
    {
        // This is bad, but not until 2038
        public static readonly Random random = new Random((int)DateTime.Now.Ticks);

        public static bool IsOnScreen(Vector2 point)
        {
            return point.X >= 0 && point.X <= Game.Instance.DisplayWidth && point.Y >= 0 && point.Y <= Game.Instance.DisplayHeight;
        }
    }
}
