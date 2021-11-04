using System;
using QuoridorDelta.Controller;

namespace QuoridorDelta.ViewConsole
{
    public static class ViewConsole
    {
        public static void Main()
        {
            try
            {
                Run();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);

                throw;
            }
        }

        private static void Run()
        {
            // new Tester().Test();
            
            var gameInput = new GameInput();
            var gameView = new GameView(gameInput);
            
            new Game().Start(gameInput, gameView);
        }
    }
}
