using System;

namespace Quoridor.Model
{
    public sealed class Quoridor
    {
        public Field Board { get; private set; }
        public Pawn Player1 { get; private set; }
        public Pawn Player2 { get; private set; }

        public Quoridor()
        {
            
        }

    }
}
