using UnityEngine;
using QuoridorDelta.Model;

namespace QuoridorDelta.View
{
    public sealed class RaycastToDesk
    {
        private Camera _camera;
        private LayerMask _layerForRaycast;
        private CoordsConverter _coordsConverter;

        public RaycastToDesk(Camera camera, LayerMask layerMask, CoordsConverter coordsConverter)
        {
            _camera = camera;
            _layerForRaycast = layerMask;
            _coordsConverter = coordsConverter;
        }

        public bool TryGetPawnMoveCoords(out Coords coords)
        {
            Physics.Raycast(_camera.ScreenPointToRay(Input.mousePosition), out RaycastHit hit);

            if (hit.transform && 
                hit.transform.gameObject.layer == _layerForRaycast.value && 
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
            Physics.Raycast(_camera.ScreenPointToRay(Input.mousePosition), out RaycastHit hit);

            if (hit.transform && 
                hit.transform.gameObject.layer == _layerForRaycast.value && 
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