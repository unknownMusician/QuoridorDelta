using System;
using System.Collections.Generic;

namespace Quoridor.Model
{
    public sealed class Board
    {
        private const int _defaultBoardSize = 9;
        private int _size;
        private Cell[,] _cells;
        private Wall[,] _walls;

        public Pawn Player1 { get; private set; }
        public Pawn Player2 { get; private set; }

        public Board(int size)
        {
            if (size / 2 == 0)
            {
                throw new Exception("Size of board should be odd number");
            }
            _size = size;
            _cells = new Cell[_size, _size];
        }
        public Board() : this(_defaultBoardSize) { }

        //private void InitializeBoard(int boardSize)
        //{
        //    _size = boardSize;
        //    _cells = new Cell[_size, _size];
        //}
        public Cell GetCell(int xPosition, int yPosition) => _cells[xPosition, yPosition];

        private void SetupBoard()
        {
            _cells = new Cell[_size, _size];
            for (int x = 0; x < _cells.GetLength(0); x++)
            {
                for (int y = 0; y < _cells.GetLength(1); y++)
                {
                    _cells[x, y] = new Cell(false, x, y);
                }
            }
        }
    }
}
