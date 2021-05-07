using System;
using System.Collections.Generic;
using System.Text;

namespace Swing
{
    public enum Tiles
    {
        Nothing = 0,
        Wall = 1,
        UpSpike = 2,
        Goal = 3,
        PlayerSpawn = 4,
        RightSpike = 5,
        LeftSpike = 6,
        DownSpike = 7,
        RightBooster = 9,
        DownBooster = 10,
        LeftBooster = 11,
        UpBooster = 12,
        UpRightSpike = 13,
        UpLeftSpike = 14,
        LeftUpRightSpike = 15,
    }

    enum ColliderTags
    {
        Wall,
        Player,
        Spike,
        Goal,
        LeftBooster,
        RightBooster,
        UpBooster,
        DownBooster,
    }
}
