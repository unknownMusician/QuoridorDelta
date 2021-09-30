using System;

namespace Quoridor.Model
{
    public sealed class Quoridor
    {
        public Board Board { get; private set; }
        public Pawn Player1 { get; private set; }
        public Pawn Player2 { get; private set; }

        public Quoridor()
        {
            Board = new Board();
            Cell firstPlayerStartPosition, secondPlayerStartPosition;
            Board.GetPawnsStartPositions(Board, out firstPlayerStartPosition, out secondPlayerStartPosition);
            Player1 = new Pawn(firstPlayerStartPosition);
            Player2 = new Pawn(secondPlayerStartPosition);
        }
        
    }
}
