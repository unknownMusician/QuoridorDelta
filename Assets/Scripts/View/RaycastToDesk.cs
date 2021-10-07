using QuoridorDelta.Model;
using UnityEngine;

namespace QuoridorDelta.View
{
    public sealed class RaycastToDesk
    {
        private readonly Camera _camera;
        private readonly LayerMask _layerForRaycast;
        private readonly CoordsConverter _coordsConverter;
        private readonly float _maxDistance;

        public RaycastToDesk(Camera camera, LayerMask layerMask, float maxDistance, CoordsConverter coordsConverter)
        {
            _camera = camera;
            _layerForRaycast = layerMask;
            _coordsConverter = coordsConverter;
            _maxDistance = maxDistance;
        }

        public bool TryGetPawnMoveCoords(out Coords coords)
        {
            if (Physics.Raycast(_camera.ScreenPointToRay(Input.mousePosition),
                                out RaycastHit hit,
                                _maxDistance,
                                _layerForRaycast) &&
             hit.normal == Vector3.up)
            {
                coords = _coordsConverter.ToCoords(hit.point);
                return true;
            }

            coords = default;

            return false;
        }

        public bool TryGetPlaceWallCoords(out WallCoords coords)
        {
            if (Physics.Raycast(_camera.ScreenPointToRay(Input.mousePosition),
                                out RaycastHit hit,
                                _maxDistance,
                                _layerForRaycast)
             && hit.normal == Vector3.up)
            {
                coords = _coordsConverter.ToWallCoords(hit.point);

                //Debug.Log($"{coords.Coords.X}:{coords.Coords.Y}:{coords.Orientation}");
                return true;
            }

            coords = default;

            return false;
        }
        // remove
        public bool TryRaycast(out Collider collider, out RaycastHit hit)
        {
            hit = default;
            collider = default;
            if (Physics.Raycast(_camera.ScreenPointToRay(Input.mousePosition), out RaycastHit _hit, _maxDistance))
            {
                hit = _hit;
                collider = hit.collider;
                return true;
            }
            return false;
        }
        public bool TryRaycast(out RaycastHit hit)
        {
            hit = default;
            if (Physics.Raycast(_camera.ScreenPointToRay(Input.mousePosition), out RaycastHit _hit, _maxDistance))
            {
                hit = _hit;
                return true;
            }
            return false;
        }

        public bool TryGetCollider(out Collider collider)
        {
            if (Physics.Raycast(_camera.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, _maxDistance))
            {
                collider = hit.collider;
                return true;
            }
            collider = null;
            return false;
        }
    }
}
