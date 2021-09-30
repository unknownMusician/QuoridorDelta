using UnityEngine;
using Quoridor.Model;
using System;

namespace QuoridorDelta.Quoridor
{
    public class RaycastToDesk : MonoBehaviour
    {
        [SerializeField] private LayerMask _layerForRaycast;
        private Camera _camera;

        void Awake()
        {
            _camera = GetComponent<Camera>();
        }

        void Update()
        {
            if (Input.GetMouseButton(0))
            {
                RaycastHit hit = default;
                Physics.Raycast(_camera.ScreenPointToRay(Input.mousePosition), out hit);
                Coords coords = GetWallCoords(hit);
                Debug.Log($"{coords.X}:{coords.Y}");
            }
        }
        public Coords GetPawnCoords(RaycastHit hit)
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
        public Coords GetWallCoords(RaycastHit hit)
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