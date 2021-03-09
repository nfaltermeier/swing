using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Swing.Engine
{
    static class Debug
    {
        public static readonly bool DISPLAY_COLLIDERS = true;

        public static void Log(string output,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            System.Diagnostics.Debug.WriteLine($"{sourceFilePath}: {memberName}({sourceLineNumber}): {output}");
        }

        public static void LogError(string output,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            System.Diagnostics.Debug.WriteLine($"{sourceFilePath}: {memberName}({sourceLineNumber}) ERROR: {output}");
        }
    }
}
