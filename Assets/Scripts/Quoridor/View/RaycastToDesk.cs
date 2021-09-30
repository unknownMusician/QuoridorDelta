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
                Coords coords = GetCoords(hit);
                Debug.Log($"{coords.X}:{coords.Y}");
            }
        }
        public Coords GetCoords(RaycastHit hit)
        {
            if (hit.transform.gameObject.layer == _layerForRaycast.value &&
                hit.normal == Vector3.up)
            {
                //Debug.Log($"{hit.point}");
                Vector3 startPoint = Vector3.zero;
                Vector3 point = startPoint + hit.point;
                Coords coords = new Coords((int)point.x, (int)point.z);
                return coords;
            }
            return null;
        }
    }
}