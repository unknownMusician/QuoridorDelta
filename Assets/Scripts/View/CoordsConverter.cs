using QuoridorDelta.Model;
using UnityEngine;

namespace QuoridorDelta.View
{
    public sealed class CoordsConverter
    {
        private readonly Vector3 _boardStartPoint;
        private readonly Vector3 _wallStartPoint;
        private readonly Vector3 _centerPoint;
        private const int BoardSize = 9;
        private const float PawnHeightValue = 0.652f;
        private const float WallHeightValue = 0.9f;

        public CoordsConverter(Vector3 centerPoint)
        {
            _centerPoint = centerPoint;
            _boardStartPoint = new Vector3(centerPoint.x - 4.5f, 0, centerPoint.z - 4.5f);
            _wallStartPoint = new Vector3(centerPoint.x - 4f, 0, centerPoint.z - 4f);
        }

        private static Coords ToCoords(Vector3 pointInWorld, Vector3 startPoint, int maxClampValue)
        {
            Vector3 point = pointInWorld - startPoint;

            return new Coords(Mathf.Clamp(Mathf.FloorToInt(point.x), 0, maxClampValue),
                              Mathf.Clamp(Mathf.FloorToInt(point.z), 0, maxClampValue));
        }

        private static WallRotation GetWallOrientation(Vector3 coordsInOwnCoordSystem)
        {
            float absCoordsInOwnCoordSystemZ = Mathf.Abs(coordsInOwnCoordSystem.z);

            return (absCoordsInOwnCoordSystemZ < coordsInOwnCoordSystem.x
                 || absCoordsInOwnCoordSystemZ < -coordsInOwnCoordSystem.x) ?
                WallRotation.Horizontal :
                WallRotation.Vertical;
        }

        public Coords ToCoords(Vector3 pointInWorld) => ToCoords(pointInWorld, _boardStartPoint, BoardSize - 1);

        public WallCoords ToWallCoords(Vector3 pointInWorld)
        {
            Vector3 point = pointInWorld - _wallStartPoint;

            var pointInNewSystem = new Vector3(point.x - (Mathf.FloorToInt(point.x) + 0.5f),
                                               0,
                                               point.z - (Mathf.FloorToInt(point.z) + 0.5f));

            WallRotation wallOrientation = GetWallOrientation(pointInNewSystem);

            return new WallCoords(ToCoords(pointInWorld, _wallStartPoint, BoardSize - 2), wallOrientation);
        }

        public Vector3 ToVector3(Coords coords)
        {
            // todo
            //(int x, int y) = coords;
            int x = coords.X;
            int y = coords.Y;

            return new Vector3(x + _centerPoint.x + 0.5f, PawnHeightValue, y + _centerPoint.z + 0.5f)
                 + _boardStartPoint;
        }

        public Vector3 ToVector3(WallCoords wallCoords)
        {
            // todo
            //(int x, int y) = wallCoords.Coords;
            int x = wallCoords.Coords.X;
            int y = wallCoords.Coords.Y;

            return new Vector3(x + _centerPoint.x + 0.5f, WallHeightValue, y + _centerPoint.z + 0.5f) + _wallStartPoint;
        }
    }
}
