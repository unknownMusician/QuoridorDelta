using System;
using QuoridorDelta.Model;
using UnityEngine;

namespace QuoridorDelta.View
{
    public sealed class CoordsConverter : MonoBehaviour
    {
        [SerializeField] private Vector3 _centerPoint;
        
        private Vector3 _cellStartPoint;
        private Vector3 _wallStartPoint;
        public const int BoardCellSize = 9;
        public const int BoardWallSize = BoardCellSize - 1;
        public const float PawnHeightValue = 0.652f;
        public const float WallHeightValue = 0.9f;
        public const float BoardCellHalfSize = BoardCellSize / 2.0f;
        public const float BoardWallHalfSize = BoardWallSize / 2.0f;
        public Vector3 CenterPoint => _centerPoint;

        public void Awake()
        {
            Vector3 flatCenterPoint = DropY(_centerPoint);

            var boardCellOffset = new Vector3(BoardCellHalfSize, 0, BoardCellHalfSize);
            var boardWallOffset = new Vector3(BoardWallHalfSize, 0, BoardWallHalfSize);

            _cellStartPoint = flatCenterPoint - boardCellOffset;
            _wallStartPoint = flatCenterPoint - boardWallOffset;
        }

        private static Vector3 DropY(Vector3 v) => new Vector3(v.x, 0, v.z);

        private static Coords ToCoords(Vector3 worldPosition, Vector3 startPoint, int maxClampValue)
        {
            Vector3 point = worldPosition - startPoint;

            return new Coords(Mathf.Clamp(Mathf.FloorToInt(point.x), 0, maxClampValue),
                              Mathf.Clamp(Mathf.FloorToInt(point.z), 0, maxClampValue));
        }

        private static WallRotation GetWallRotation(Vector3 coordsInOwnCoordSystem)
            => Mathf.Abs(coordsInOwnCoordSystem.z) < Mathf.Abs(coordsInOwnCoordSystem.x) ?
                WallRotation.Horizontal :
                WallRotation.Vertical;

        public Coords ToCoords(Vector3 worldPosition)
            => ToCoords(worldPosition, _cellStartPoint, BoardCellSize - 1);

        public WallCoords ToWallCoords(Vector3 pointInWorld)
        {
            Vector3 point = pointInWorld - _wallStartPoint;

            var pointInNewSystem = new Vector3((point.x - Mathf.Floor(point.x)) - 0.5f,
                                               0,
                                               (point.z - Mathf.Floor(point.z)) - 0.5f);

            return new WallCoords(ToCoords(pointInWorld, _wallStartPoint, BoardWallSize - 1),
                                  GetWallRotation(pointInNewSystem));
        }

        public Vector3 ToVector3(Coords coords)
        {
            (int x, int y) = coords;

            return new Vector3(x + CenterPoint.x + 0.5f, PawnHeightValue, y + CenterPoint.z + 0.5f) + _cellStartPoint;
        }

        public Vector3 ToVector3(WallCoords wallCoords)
        {
            (int x, int y) = wallCoords.Coords;

            return new Vector3(x + CenterPoint.x + 0.5f, WallHeightValue, y + CenterPoint.z + 0.5f) + _wallStartPoint;
        }

        public Quaternion GetWallQuaternion(WallRotation wallRotation) => wallRotation switch
        {
            WallRotation.Vertical => Quaternion.identity,
            WallRotation.Horizontal => Quaternion.Euler(0, 90.0f, 0),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}
