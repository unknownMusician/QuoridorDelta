using System;

namespace Quoridor.Model
{
    public sealed class Cell
    {
        public readonly int XPosition;
        public readonly int YPosition;

        public bool ContainsPawn { get; private set; }

        public Cell(int xPos, int yPos)
        {
            ContainsPawn = false;
            XPosition = xPos;
            YPosition = yPos;
        }
    }
}
