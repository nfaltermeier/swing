using System;
using System.Collections.Generic;
using System.Text;

namespace Swing
{
    enum Tiles
    {
        Nothing = 0,
        Wall = 1,
        Spike = 2,
        Goal = 3,
        PlayerSpawn = 4,
    }

    enum ColliderTags
    {
        Wall,
        Player,
        Spike,
        Goal,
    }
}
