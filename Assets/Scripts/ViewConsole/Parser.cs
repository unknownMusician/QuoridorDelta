#nullable enable

using System;
using QuoridorDelta.Controller;
using QuoridorDelta.Model;

namespace QuoridorDelta.ViewConsole
{
    public sealed class Parser
    {
        private const string WallStringKey = "STUVWXYZ";
        private const string PawnStringKey = "ABCDEFGHI";


        public Coords ParseStringToPawnCoords(string commandStr)
        {
            string[] strings = ParseCommandToStringArray(commandStr);

            return ConvertStringCoordsToObjectCoords(strings[1], Parser.PawnStringKey);
        }

        public MoveType ParseStringToMoveType(string commandStr)
        {
            string[] strings = ParseCommandToStringArray(commandStr);

            return GetMoveType(strings[0]);
        }

        public string ParseNewCoordsAndMoveTypeToCommandString(Coords coords)
        {
            bool wasJump = false; // todo boolean check 
            string moveStr = "move";

            if (wasJump)
            {
                moveStr = "jump";
            }

            return moveStr + " " + Parser.PawnStringKey[coords.X] + (9 - coords.Y);
        }

        public string ParseNewCoordsAndMoveTypeToCommandString(WallCoords wallCoords)
        {
            char rotation = wallCoords.Rotation switch
            {
                WallRotation.Vertical => 'v',
                WallRotation.Horizontal => 'h',
                _ => throw new ArgumentOutOfRangeException()
            };

            return "wall " + Parser.WallStringKey[wallCoords.Coords.X] + (8 - wallCoords.Coords.Y) + rotation;
        }


        public WallCoords ParseStringToWallCoords(string commandStr)
        {
            string[] strings = ParseCommandToStringArray(commandStr);

            return ConvertStringCoordsToObjectWallCoords(strings[1], Parser.WallStringKey);
        }

        private string[] ParseCommandToStringArray(string commandStr)
        {
            // Can make checks of inputed string
            return commandStr.Split(' ');
        }

        private Coords ConvertStringCoordsToObjectCoords(string coordsStr, string key)
        {
            int x = key.IndexOf(coordsStr[0]);
            int y = 9 - int.Parse(coordsStr[1].ToString());

            return new Coords(x, y);
        }

        private WallCoords ConvertStringCoordsToObjectWallCoords(string wallCoordsStr, string key)
        {
            return new WallCoords(ConvertStringCoordsToObjectCoords(wallCoordsStr, key),
                                  GetWallRotation(wallCoordsStr[2]));
        }


        private MoveType GetMoveType(string moveTypeStr)
        {
            MoveType moveType = MoveType.PlaceWall;

            if (moveTypeStr == "move" || moveTypeStr == "jump")
            {
                moveType = MoveType.MovePawn;
            }

            return moveType;
        }

        private WallRotation GetWallRotation(Char wallRotationStr)
        {
            // todo Refactor
            WallRotation wallRotation = WallRotation.Horizontal;

            if (wallRotationStr == 'v')
            {
                wallRotation = WallRotation.Vertical;
            }

            return wallRotation;
        }
    }
}
