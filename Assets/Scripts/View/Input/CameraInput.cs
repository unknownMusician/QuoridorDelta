using UnityEngine;

namespace QuoridorDelta.View
{
    public sealed class CameraInput : MonoBehaviour
    {
        private const int RightMouseButtonIndex = 1;
        [SerializeField] private float _speed = 0.1f;
        [SerializeField] private float _minEulerX = -55.0f;
        [SerializeField] private float _maxEulerX = 1.0f;
        [SerializeField] private float _slerp = 0.1f;

        private Vector2 _lastMouseClickPosition;
        private Vector2 _cameraEuler;
        private Quaternion _targetRotation;

        private void Update()
        {
            if (Input.GetMouseButtonDown(RightMouseButtonIndex))
            {
                _lastMouseClickPosition = Input.mousePosition;
            }

            Vector2 mousePosition = ((Vector2)Input.mousePosition - _lastMouseClickPosition) * _speed;

            Vector2 newEuler = ClampX(new Vector2(_cameraEuler.x - mousePosition.y, _cameraEuler.y + mousePosition.x),
                                      _minEulerX,
                                      _maxEulerX);

            if (Input.GetMouseButton(RightMouseButtonIndex))
            {
                _targetRotation = Quaternion.Euler(newEuler.x, newEuler.y, 0);
            }
            else if (Input.GetMouseButtonUp(RightMouseButtonIndex))
            {
                _cameraEuler = newEuler;
            }

            transform.rotation = Quaternion.Slerp(transform.rotation, _targetRotation, _slerp);
        }

        private Vector2 ClampX(Vector2 vector, float min, float max)
            => new Vector2(Mathf.Clamp(vector.x, min, max), vector.y);
    }
}
