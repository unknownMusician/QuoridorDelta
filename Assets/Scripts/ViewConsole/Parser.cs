using System;
using QuoridorDelta.Controller;
using QuoridorDelta.Model;

namespace QuoridorDelta.ViewConsole
{
    public sealed class Parser
    {
        private const string WallStringKey = "STUVWXYZ";
        private const string PawnStringKey = "ABCDEFGHI";


        public Coords ParsePawnCoords(string command) => ParsePawnCoords(ParseCommandToStringArray(command));

        public Coords ParsePawnCoords(string[] splittedStrings)
            => ConvertStringCoordsToObjectCoords(splittedStrings[1], Parser.PawnStringKey);

        public MoveType ParseMoveType(string command) => ParseMoveType(ParseCommandToStringArray(command));

        public MoveType ParseMoveType(string[] splittedStrings) => GetMoveType(splittedStrings[0]);

        public PlayerNumber ParsePlayerNumber(string command)
            => ParsePlayerNumber(ParseCommandToStringArray(command));

        public PlayerNumber ParsePlayerNumber(string[] splittedStrings) => GetPlayerNumber(splittedStrings[0]);

        public string ParseNewCoordsAndMoveTypeToCommandString(Coords coords, bool isJump)
        {
            string moveStr = isJump ? "jump" : "move";

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
            => ParseStringToWallCoords(ParseCommandToStringArray(commandStr));

        public WallCoords ParseStringToWallCoords(string[] splittedStrings)
            => ConvertStringCoordsToObjectWallCoords(splittedStrings[1], Parser.WallStringKey);

        public string[] ParseCommandToStringArray(string commandStr)
        {
            // Can make checks of inputed string
            return commandStr.Split(' ');
        }

        private Coords ConvertStringCoordsToObjectCoords(string coordsStr, string key)
            => (key.IndexOf(coordsStr[0]), 9 - int.Parse(coordsStr[1].ToString()));

        private WallCoords ConvertStringCoordsToObjectWallCoords(string wallCoordsStr, string key)
        {
            Coords coords = ConvertStringCoordsToObjectCoords(wallCoordsStr, key);
            coords = (coords.X, coords.Y - 1);

            return new WallCoords(coords, GetWallRotation(wallCoordsStr[2]));
        }

        private MoveType GetMoveType(string moveType) => moveType switch
        {
            "move" => MoveType.MovePawn,
            "jump" => MoveType.MovePawn,
            "wall" => MoveType.PlaceWall,
            _ => throw new ArgumentOutOfRangeException()
        };

        private WallRotation GetWallRotation(char wallRotation) => wallRotation switch
        {
            'v' => WallRotation.Vertical,
            'h' => WallRotation.Horizontal,
            _ => throw new ArgumentOutOfRangeException()
        };

        private PlayerNumber GetPlayerNumber(string playerNumber) => playerNumber switch
        {
            "white" => PlayerNumber.First,
            "black" => PlayerNumber.Second,
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}
