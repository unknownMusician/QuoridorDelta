using QuoridorDelta.Model;

namespace QuoridorDelta.View
{
    public interface IBot : IInput
    {
        public Coords GetRandomPawnCoords(Coords[] possibleCoords);
        public WallCoords GetRandomWallCoords(WallCoords[] possibleWallPlacementCoords);

    }
}