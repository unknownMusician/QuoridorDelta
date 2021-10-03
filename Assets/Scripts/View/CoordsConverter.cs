using QuoridorDelta.Model;
using System;
using UnityEngine;

namespace QuoridorDelta.View
{
    public class CoordsConverter
    {
        private Vector3 _boardStartPoint;
        private Vector3 _wallStartPoint;
        private const int _boardSize = 9;
        private const float _pawnHeightValue = 0.652f;
        private const float _wallHeightValue = 0.9f;
        private Vector3 _centerPoint;

        public CoordsConverter(Vector3 centerPoint)
        {
            _centerPoint = centerPoint;
            _boardStartPoint = new Vector3(centerPoint.x - 4.5f, 0, centerPoint.z - 4.5f);
            _wallStartPoint = new Vector3(centerPoint.x - 4f, 0, centerPoint.z - 4f);
        }
        
        private Coords ToCoords(Vector3 pointInWorld, Vector3 startPoint, int maxClampValue)
        {
            Vector3 point = pointInWorld - startPoint;
            return new Coords(
                Mathf.Clamp((int)point.x, 0, maxClampValue),
                Mathf.Clamp((int)point.z, 0, maxClampValue));
        }
        private WallOrientation GetWallOrientation(Vector3 coordsInOwnCoordSystem)
        {
            return (Mathf.Abs(coordsInOwnCoordSystem.z) < coordsInOwnCoordSystem.x ||
                Mathf.Abs(coordsInOwnCoordSystem.z) < -coordsInOwnCoordSystem.x) ?
                WallOrientation.Horizontal : WallOrientation.Vertical;
        }

        public Coords ToCoords(Vector3 pointInWorld) => ToCoords(pointInWorld, _boardStartPoint, _boardSize - 1);
        public WallCoords ToWallCoords(Vector3 pointInWorld)
        {
            Vector3 point = pointInWorld - _wallStartPoint;
            Vector3 pointInNewSystem = new Vector3(point.x - ((int)point.x + 0.5f), 0, point.z - ((int)point.z + 0.5f));
            WallOrientation wallOrientation = GetWallOrientation(pointInNewSystem);

            return new WallCoords(
                ToCoords(pointInWorld, _wallStartPoint, _boardSize - 2),
                wallOrientation);
        }
        public Vector3 ToVector3(Coords coords)
        {
            return new Vector3(
                coords.X + _centerPoint.x + 0.5f, 
                _pawnHeightValue, 
                coords.Y + _centerPoint.z + 0.5f) + _boardStartPoint;
        }
        public Vector3 ToVector3(WallCoords wallCoords)
        {
            return new Vector3(
                wallCoords.Coords.X + _centerPoint.x + 0.5f, 
                _wallHeightValue, 
                wallCoords.Coords.Y + _centerPoint.z + 0.5f) 
                + _wallStartPoint;
        }
    }
}
