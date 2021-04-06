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
            return point.X >= 0 && point.X <= MainGame.Instance.DisplayWidth && point.Y >= 0 && point.Y <= MainGame.Instance.DisplayHeight;
        }

        public static float NextFloat(this Random random)
        {
            return (float)random.NextDouble();
        }

        public static float NextFloat(this Random random, float minValue, float maxValue)
        {
            return minValue + (float)random.NextDouble() * (maxValue - minValue);
        }
        public static Vector2 NextDirection(this Random random)
        {
            float angle = random.NextFloat(0, MathHelper.TwoPi);
            return new Vector2(MathF.Cos(angle), MathF.Sin(angle));
        }

        public static Vector2 PositiveNextDirection(this Random random)
        {
            float angle = random.NextFloat(0, MathHelper.Pi);
            return new Vector2(MathF.Cos(angle), MathF.Sin(angle));
        }

        public static Vector2 NegativeNextDirection(this Random random)
        {
            float angle = random.NextFloat(MathHelper.Pi, MathHelper.TwoPi);
            return new Vector2(MathF.Cos(angle), MathF.Sin(angle));
        }

        public static Vector2 RandomDirection(this Random random, float minAngle, float maxAngle)
        {
            float angle = random.NextFloat(minAngle, maxAngle);
            return new Vector2(MathF.Cos(angle), MathF.Sin(angle));
        }

        public static Vector2 RandomPosition(this Random random, Rectangle bounds)
        {
            return new Vector2(
                random.NextFloat(bounds.X, bounds.X + bounds.Width),
                random.NextFloat(bounds.Y, bounds.Y + bounds.Height)
                );
        }
    }
}
