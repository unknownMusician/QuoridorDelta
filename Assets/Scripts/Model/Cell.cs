using System;

namespace Quoridor.Model
{
    public sealed class Cell
    {
        private int _xPosition;
        private int _yPosition;

        public bool ContainsPawn { get; set; }
        public int XPositon => _xPosition;
        public int YPositon => _yPosition;

        public Cell(bool isPawned, int xPos, int yPos)
        {
            ContainsPawn = isPawned;
            _xPosition = xPos;
            _yPosition = yPos;
        }
    }
}
