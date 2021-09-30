using System;

namespace Quoridor.Model
{
    public sealed class Pawn
    {
        private Cell _currentCell;
        private int _leftWalls = 10;

        public string Name { get; }
        public int WallsCount => _leftWalls;

        public Pawn(string name)
        {
            Name = name;
        }
    }
}
