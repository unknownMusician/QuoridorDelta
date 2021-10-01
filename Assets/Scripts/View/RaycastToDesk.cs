using UnityEngine;
using QuoridorDelta.Model;

namespace QuoridorDelta.View
{
    public class RaycastToDesk : MonoBehaviour
    {
        [SerializeField] private LayerMask _layerForRaycast;
        private Camera _camera;

        // use [RequireComponent(typeof(Camera))] attribute
        private void Awake() => _camera = GetComponent<Camera>();

        private void Update()
        {
            if (Input.GetMouseButton(0))
            {
                Physics.Raycast(_camera.ScreenPointToRay(Input.mousePosition), out RaycastHit hit);
                Coords? coords = GetWallCoords(hit);
                // needs null check or *
                Debug.Log($"{coords.Value.X}:{coords.Value.Y}");
            }
        }
        // * or do not use nullable
        public Coords? GetPawnCoords(RaycastHit hit)
        {
            if (hit.transform.gameObject.layer == _layerForRaycast.value &&
                hit.normal == Vector3.up)
            {
                Vector3 startPoint = Vector3.zero;
                Vector3 point = startPoint + hit.point;
                Coords coords = new Coords((int)point.x, (int)point.z);
                return coords;
            }
            return null;
        }
        public Coords? GetWallCoords(RaycastHit hit)
        {
            if (hit.transform.gameObject.layer == _layerForRaycast.value &&
                hit.normal == Vector3.up)
            {
                Vector3 startPoint = new Vector3(1, 0, 1);
                Vector3 point = hit.point - startPoint;
                Coords coords = new Coords((int)point.x, (int)point.z);
                return coords;
            }
            return null;
        }
    }
}