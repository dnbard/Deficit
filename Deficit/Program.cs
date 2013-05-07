#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
#endregion

namespace Deficit
{
#if WINDOWS || LINUX
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Random = new Random();

            Game = new DeficitGame();
            Game.Run();
        }

        public static DeficitGame Game { get; private set; }

        public static Random Random { get; private set; }
    }
#endif
}
