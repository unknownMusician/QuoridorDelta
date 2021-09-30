using System;

namespace Quoridor.Model
{
    public sealed class Board
    {
        private const int _defaultBoardSize = 9;
        private Cell[,] _cellGrid;
        private WallSlot[,] _wallGrid;

        public readonly int Size;

        public Board(int size)
        {
            if (size / 2 == 0)
            {
                throw new Exception("Size of board should be odd number");
            }
            Size = size;
            SetupCells();
            SetupWallSlots();
        }
        public Board() : this(_defaultBoardSize) { }

        private void SetupCells()
        {
            _cellGrid = new Cell[Size, Size];
            for (int x = 0; x < _cellGrid.GetLength(0); x++)
            {
                for (int y = 0; y < _cellGrid.GetLength(1); y++)
                {
                    _cellGrid[x, y] = new Cell(x, y);
                }
            }
        }
        private void SetupWallSlots()
        {
            _wallGrid = new WallSlot[Size - 1, Size - 1];
            for (int x = 0; x < _wallGrid.GetLength(0); x++)
            {
                for (int y = 0; y < _wallGrid.GetLength(1); y++)
                {
                    _wallGrid[x, y] = new WallSlot(x, y);
                }
            }
        }
        
        public Cell GetCell(int xPosition, int yPosition) => _cellGrid[xPosition, yPosition];
        public WallSlot GetWallSlot(int xPosition, int yPosition) => _wallGrid[xPosition, yPosition];

        //public static void GetPawnsStartPositions(Board board, out Cell player1Pos, out Cell player2Pos)
        //{
        //    player1Pos = board._cellGrid[board._cellGrid.GetLength(0) / 2, 0];
        //    player2Pos = board._cellGrid[board._cellGrid.GetLength(0) / 2, board._cellGrid.GetLength(1)]; //wrong index
        //}
    }
}
