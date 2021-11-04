using System;
using QuoridorDelta.Model;

namespace QuoridorDelta.ViewConsole
{
    public class Tester
    {
        private readonly Parser _parser = new Parser();

        public void Test()
        {
            Console.WriteLine(_parser.ParseMoveType("move E8"));
            Console.WriteLine(_parser.ParseMoveType("wall U7h"));
            Console.WriteLine(_parser.ParseStringToWallCoords("wall U8h"));
            Console.WriteLine(_parser.ParsePawnCoords("move E7"));
            Console.WriteLine(_parser.ParseNewCoordsAndMoveTypeToCommandString((1, 2), true));
            Console.WriteLine(_parser.ParseNewCoordsAndMoveTypeToCommandString((1, 2), false));
            Console.WriteLine(_parser.ParseNewCoordsAndMoveTypeToCommandString(((1, 2), WallRotation.Horizontal)));
        }
    }
}
