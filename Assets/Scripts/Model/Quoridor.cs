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
            Board = new Field();
            Player1 = new Pawn(Board.GetCell(Board.Size / 2, 0));
            Player2 = new Pawn(Board.GetCell(Board.Size / 2, Board.Size - 1));
        }
        
    }
}
