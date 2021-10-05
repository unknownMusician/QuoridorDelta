using System.Collections.Generic;

namespace PossibleRefactor.Model
{
    public readonly struct Coords
    {
        public readonly int X;
        public readonly int Y;

        public Coords(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}