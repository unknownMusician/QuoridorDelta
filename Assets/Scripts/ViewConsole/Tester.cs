using System;
using QuoridorDelta.Model;

namespace ViewConsole
{
    public class Tester
    {
        private Parser _parser = new Parser();
        
        public void test()
        {
            Console.WriteLine(_parser.ParseStringToMoveType("move E8"));
            Console.WriteLine(_parser.ParseStringToMoveType("wall U7h"));
            Console.WriteLine(_parser.ParseStringToWallCoords("wall U8h"));
            Console.WriteLine(_parser.ParseStringToPawnCoords("move E7"));
            Console.WriteLine(_parser.ParseNewCoordsAndMoveTypeToCommandString(new Coords(1,2)));
            Console.WriteLine(
                _parser.ParseNewCoordsAndMoveTypeToCommandString(new WallCoords(new Coords(1, 2),
                    WallRotation.Horizontal)));
        }
    }
}