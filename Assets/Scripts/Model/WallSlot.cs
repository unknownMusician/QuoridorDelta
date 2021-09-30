using System;

namespace Quoridor.Model
{
    public sealed class WallSlot
    {
        public readonly int XPosition;
        public readonly int YPosition;

        public Wall Wall { get; private set; }
        //public bool ContainsWall { get; private set; }

        public WallSlot(int xPos, int yPos)
        {
            //ContainsWall = false;
            XPosition = xPos;
            YPosition = yPos;
        }

        public bool PutWall(WallOrientation orientation)
        {
            if (Wall != null)
            {
                //Notify ("There is already a wall here");
                return false;
            }
            Wall = new Wall(orientation);
            return true;
        }
    }
}
