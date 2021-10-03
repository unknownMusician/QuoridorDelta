using UnityEngine;
using QuoridorDelta.Model;

namespace QuoridorDelta.View
{
    public sealed class RaycastToDesk
    {
        private Camera _camera;
        private LayerMask _layerForRaycast;
        private CoordsConverter _coordsConverter;
        private float _maxDistance;

        public RaycastToDesk(Camera camera, LayerMask layerMask, float maxDistance, CoordsConverter coordsConverter)
        {
            _camera = camera;
            _layerForRaycast = layerMask;
            _coordsConverter = coordsConverter;
            _maxDistance = maxDistance;
        }

        public bool TryGetPawnMoveCoords(out Coords coords)
        {
            if (Physics.Raycast(_camera.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, _maxDistance, _layerForRaycast) && 
                hit.normal == Vector3.up)
            {
                coords = _coordsConverter.ToCoords(hit.point);
                //Debug.Log($"{coords.X}:{coords.Y}");
                return true;
            }
            coords = default;
            return false;
        }
        public bool TryGetPlaceWallCoords(out WallCoords coords)
        {
            if (Physics.Raycast(_camera.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, _maxDistance, _layerForRaycast) && 
                hit.normal == Vector3.up)
            {
                coords = _coordsConverter.ToWallCoords(hit.point);
                //Debug.Log($"{coords.Coords.X}:{coords.Coords.Y}:{coords.Orientation}");
                return true;
            }
            coords = default;
            return false;
        }
    }
}