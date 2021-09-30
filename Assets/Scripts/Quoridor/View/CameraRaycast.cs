using UnityEngine;

namespace QuoridorDelta.Quoridor
{
    public class CameraRaycast : MonoBehaviour
    {
        [SerializeField] private LayerMask _layerForRaycast;
        private Camera _camera;

        void Awake()
        {
            _camera = GetComponent<Camera>();
        }

        void Update()
        {
            MakeRaycast();
        }
        private void MakeRaycast()
        {
            RaycastHit hit;
            if (Input.GetMouseButton(0) && 
                Physics.Raycast(_camera.ScreenPointToRay(Input.mousePosition), out hit) &&
                hit.transform.gameObject.layer == _layerForRaycast.value &&
                hit.normal == Vector3.up)
            {
                //Debug.Log($"{hit.transform.gameObject.layer}");
                //Debug.Log($"{_layerForRaycast.value}");
                Debug.Log($"{hit.point}");
            }
        }
    }
}