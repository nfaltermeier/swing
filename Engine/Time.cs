using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Swing.Engine
{
    public static class Time
    {
        public static float TotalTime { get; set; }
        /// <summary>
        /// Automatically changes between FixedDeltaTime and UpdateDeltaTime depending on if it is called in FixedUpdate or not.
        /// </summary>
        public static float DeltaTime => IsInFixedUpdate ? FixedDeltaTime : UpdateDeltaTime;
        public static float FixedDeltaTime { get; set; } = 1 / 60f;

        // These are not for general use
        public static float UpdateDeltaTime { get; set; }
        public static bool IsInFixedUpdate { get; set; } = false;

        public static void UpdateTime(GameTime gameTime)
        {
            TotalTime = (float)gameTime.TotalGameTime.TotalSeconds;
            UpdateDeltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
    }
}
