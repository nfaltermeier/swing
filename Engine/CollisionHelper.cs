using Swing.Engine.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace Swing.Engine
{
    public static class CollisionHelper
    {
        public static bool Collides(RectangleCollider a, RectangleCollider b)
        {
            return !(a.Right < b.Left || a.Left > b.Right || a.Top < b.Bottom || a.Bottom > b.Top);
        }

        public static void CheckCollision(List<RectangleCollider> colliders)
        {
            for (int i = 0; i < colliders.Count; i++)
            {
                RectangleCollider a = colliders[i];
                if (a.IsDestroyed)
                    continue;

                for (int n = i + 1; n < colliders.Count; n++)
                {
                    RectangleCollider b = colliders[n];
                    if (b.IsDestroyed)
                        continue;

                    if (a.CollidesWith(b))
                    {
                        a.DoCollisionTriggers(b);
                    }
                }
            }
        }
    }
}
