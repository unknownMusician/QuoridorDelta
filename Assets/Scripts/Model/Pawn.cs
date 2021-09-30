using System;

namespace Quoridor.Model
{
    public sealed class Pawn
    {
        private const int _defaultWallsCount = 10;

        public int WallsCount { get; private set; }
        public Cell CurrentCell { get; private set; }

        public Pawn(int wallsCount, Cell currentCell)
        {
            WallsCount = wallsCount;
            CurrentCell = currentCell;
        }

        public Pawn(Cell currentCell) : this(_defaultWallsCount, currentCell) { }

    }
}
