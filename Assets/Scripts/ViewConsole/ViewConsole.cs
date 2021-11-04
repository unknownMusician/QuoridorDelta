#nullable enable

using QuoridorDelta.Controller;

namespace QuoridorDelta.ViewConsole
{
    public static class ViewConsole
    {
        public static void Main()
        {
            /*Tester tester = new Tester();
            tester.test();*/
            new Game().Start(new GameInput(), new GameView());
        }
    }
}
