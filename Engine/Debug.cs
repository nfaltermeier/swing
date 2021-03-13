using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using tainicom.Aether.Physics2D.Dynamics;

namespace Swing.Engine
{
    static class Debug
    {
        public static readonly bool DISPLAY_COLLIDERS = false;
        public static readonly bool DISPLAY_UI_RECTANGLES = false;
        public static readonly bool DISPLAY_PLAYER_TOUCHING_COLLIDERS = false;

        public static readonly Dictionary<Fixture, bool> playerTouchingColliders;

        static Debug()
        {
            if (DISPLAY_PLAYER_TOUCHING_COLLIDERS)
            {
                playerTouchingColliders = new Dictionary<Fixture, bool>();
            }
        }

        /// <summary>
        /// Logs a message and includes calling method, source file, and line number
        /// </summary>
        public static void Log(string message,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            LogClean($"{sourceFilePath}: {memberName}({sourceLineNumber}): {message}");
        }

        /// <summary>
        /// Logs an error message and includes calling method, source file, and line number
        /// </summary>
        public static void LogError(string message,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            LogClean($"{sourceFilePath}: {memberName}({sourceLineNumber}) ERROR: {message}");
        }
        
        /// <summary>
        /// Logs a message without any additional formatting
        /// </summary>
        public static void LogClean(string output)
        {
            System.Diagnostics.Debug.WriteLine(output);
        }

        public static void DrawRectangleOutlineScreenspace(Rectangle r)
        {
            DrawRectangleOutlineScreenspace(r, Color.LightGreen);
        }

        public static void DrawRectangleOutlineScreenspace(Rectangle r, Color color)
        {
            int iX = r.X;
            int iY = r.Y;
            int iW = r.Width;
            int iH = r.Height;

            Actor.RenderSpriteScreenspace(new Rectangle(iX, iY, 1, iH), MainGame.Instance.ScreenManager.DebugPixel, color, 1f);
            Actor.RenderSpriteScreenspace(new Rectangle(iX, iY, iW, 1), MainGame.Instance.ScreenManager.DebugPixel, color, 1f);
            Actor.RenderSpriteScreenspace(new Rectangle(iX + iW, iY, 1, iH), MainGame.Instance.ScreenManager.DebugPixel, color, 1f);
            Actor.RenderSpriteScreenspace(new Rectangle(iX, iY + iH, iW, 1), MainGame.Instance.ScreenManager.DebugPixel, color, 1f);
        }
    }
}
