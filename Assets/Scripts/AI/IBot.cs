using QuoridorDelta.Model;

namespace QuoridorDelta.View
{
    public interface IBot : IInput
    {
        public Coords[] GetRandomCoords(Coords[] possibleCoords);
        public WallCoords[] GetRandomWallCoords(WallCoords[] possibleWallPlacementCoords)

    }
}