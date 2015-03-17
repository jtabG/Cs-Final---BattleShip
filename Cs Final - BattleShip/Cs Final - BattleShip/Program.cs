using System;

namespace Cs_Final___BattleShip
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (GameFramework game = new GameFramework())
            {
                game.Run();
            }
        }
    }
#endif
}

