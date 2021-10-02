using UnityEngine;
using QuoridorDelta.Model;

namespace QuoridorDelta.View
{
    public sealed class RaycastToDesk
    {
        private Camera _camera;
        private LayerMask _layerForRaycast;

        public RaycastToDesk(Camera camera, LayerMask layerMask)
        {
            _camera = camera;
            _layerForRaycast = layerMask;
        }
        //private void Awake() => _camera = GetComponent<Camera>();

        //public void SwitchToPawnMoveSender()
        //{
        //    Debug.Log($"MovePawnInput");
        //    _input.OnLeftMouseButtonClicked += () =>
        //    {
        //        if (TryGetPawnMoveCoords(out Coords coords))
        //        {
        //            _view.SendMovePawnCoords(coords);
        //        }
        //    };
        //}
        //public void SwitchToPlaceWallSender()
        //{
        //    _input.OnLeftMouseButtonClicked += () =>
        //    {
        //        Debug.Log($"PlaceWallInput");
        //        if (TryGetPlaceWallCoords(out WallCoords coords))
        //        {
        //            _view.SendPlaceWallCoords(coords);
        //        }
        //    };
        //}
        //public bool TryMakeRaycastToDesk(out Coords coords, MoveType moveType)
        //{
        //    Physics.Raycast(_camera.ScreenPointToRay(Input.mousePosition), out RaycastHit hit);

        //    if (hit.transform.gameObject.layer == _layerForRaycast.value && hit.normal == Vector3.up)
        //    {
        //        switch (moveType)
        //        {
        //            case MoveType.MovePawn:
        //                coords = Vector3ToPawnCoords(hit.point, Vector3.zero);
        //                break;
        //            case MoveType.PlaceWall:
        //                coords = Vector3ToPawnCoords(hit.point, new Vector3(1, 0, 1));
        //                break;
        //            default:
        //                coords = default;
        //                break;
        //        }
        //        Debug.Log($"{coords.X}:{coords.Y}");
        //        return true;
        //    }
        //    coords = default;
        //    return false;
        //}
        private Coords Vector3ToPawnCoords(Vector3 hitPoint, Vector3 startPoint)
        {
            Vector3 point = hitPoint - startPoint;
            return new Coords((int)point.x, (int)point.z);
        }

        public bool TryGetPawnMoveCoords(out Coords coords)
        {
            Physics.Raycast(_camera.ScreenPointToRay(Input.mousePosition), out RaycastHit hit);

            if (hit.transform.gameObject.layer == _layerForRaycast.value && hit.normal == Vector3.up)
            {
                coords = Vector3ToPawnCoords(hit.point, Vector3.zero);
                Debug.Log($"{coords.X}:{coords.Y}");
                return true;
            }
            coords = default;
            return false;
        }
        public bool TryGetPlaceWallCoords(out WallCoords coords)
        {
            Physics.Raycast(_camera.ScreenPointToRay(Input.mousePosition), out RaycastHit hit);

            if (hit.transform.gameObject.layer == _layerForRaycast.value && hit.normal == Vector3.up)
            {
                // todo: implement choise of Wall Orientation
                coords = new WallCoords(Vector3ToPawnCoords(hit.point, new Vector3(1, 0, 1)), default);
                Debug.Log($"{coords.Coords.X}:{coords.Coords.Y}");
                return true;
            }
            coords = default;
            return false;
        }
        //private Coords? GetPawnCoords(RaycastHit hit)
        //{
        //    if (hit.transform.gameObject.layer == _layerForRaycast.value &&
        //        hit.normal == Vector3.up)
        //    {
        //        Vector3 startPoint = Vector3.zero;
        //        Vector3 point = startPoint + hit.point;
        //        Coords coords = new Coords((int)point.x, (int)point.z);
        //        return coords;
        //    }
        //    return null;
        //}
        //private Coords? GetWallCoords(RaycastHit hit)
        //{
        //    if (hit.transform.gameObject.layer == _layerForRaycast.value &&
        //        hit.normal == Vector3.up)
        //    {
        //        Vector3 startPoint = new Vector3(1, 0, 1);
        //        Vector3 point = hit.point - startPoint;
        //        Coords coords = new Coords((int)point.x, (int)point.z);
        //        return coords;
        //    }
        //    return null;
        //}
    }
}