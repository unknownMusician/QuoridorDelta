namespace Quoridor.Model
{
    public interface IRules
    {
        bool CanPlaceWall(Player player,Field field, Coords newWallCoords );
        bool CanMovePawn(Pawn pawn, Field field, Coords newCoords);
        Coords[] GetPossibleMoves(Pawn pawn, Field field);
    }
}
