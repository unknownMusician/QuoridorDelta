using UnityEngine;
using QuoridorDelta.Model;

namespace QuoridorDelta.View
{
    public sealed class RaycastToDesk
    {
        private Camera _camera;
        private LayerMask _layerForRaycast;

        private Vector3 _deskStartPoint = new Vector3(-4.5f, 0, -4.5f);
        private Vector3 _wallStartPoint = new Vector3(-4f, 0, -4f);
        private int _deskSize = 9;

        public RaycastToDesk(Camera camera, LayerMask layerMask)
        {
            _camera = camera;
            _layerForRaycast = layerMask;
        }

        private Coords Vector3ToCoords(Vector3 hitPoint, Vector3 startPoint, int maxClampValue)
        {
            Vector3 point = hitPoint - startPoint;
            return new Coords(
                Mathf.Clamp((int)point.x, 0, maxClampValue), 
                Mathf.Clamp((int)point.z, 0, maxClampValue));
        }
        private WallCoords Vector3ToWallCoords(Vector3 hitPoint, Vector3 startPoint, int maxClampValue)
        {
            Vector3 point = hitPoint - startPoint;
            Vector3 pointInNewSystem = new Vector3(point.x - ((int)point.x + 0.5f), 0, point.z - ((int)point.z + 0.5f));
            WallOrientation wallOrientation = GetWallOrientation(pointInNewSystem);

            return new WallCoords(
                Vector3ToCoords(hitPoint, startPoint, maxClampValue),
                wallOrientation);
        }
        private WallOrientation GetWallOrientation(Vector3 coordsInOwnCoordSystem)
        {
            return (Mathf.Abs(coordsInOwnCoordSystem.z) < coordsInOwnCoordSystem.x || 
                Mathf.Abs(coordsInOwnCoordSystem.z) < -coordsInOwnCoordSystem.x) ? 
                WallOrientation.Horizontal : WallOrientation.Vertical;
        }

        public bool TryGetPawnMoveCoords(out Coords coords)
        {
            Physics.Raycast(_camera.ScreenPointToRay(Input.mousePosition), out RaycastHit hit);

            if (hit.transform && 
                hit.transform.gameObject.layer == _layerForRaycast.value && 
                hit.normal == Vector3.up)
            {
                coords = Vector3ToCoords(hit.point, _deskStartPoint, _deskSize - 1);
                //Debug.Log($"{coords.X}:{coords.Y}");
                return true;
            }
            coords = default;
            return false;
        }
        public bool TryGetPlaceWallCoords(out WallCoords coords)
        {
            Physics.Raycast(_camera.ScreenPointToRay(Input.mousePosition), out RaycastHit hit);

            if (hit.transform && 
                hit.transform.gameObject.layer == _layerForRaycast.value && 
                hit.normal == Vector3.up)
            {
                coords = Vector3ToWallCoords(hit.point, _wallStartPoint, _deskSize - 2);
                //Debug.Log($"{coords.Coords.X}:{coords.Coords.Y}:{coords.Orientation}");
                return true;
            }
            coords = default;
            return false;
        }
    }
}