using System;
using UnityEngine;

namespace QuoridorDelta.View
{
    [RequireComponent(typeof(Camera))]
    public sealed class MouseInputHandler : MonoBehaviour
    {
        private const int LeftMouseButtonIndex = 0;

        private Camera _camera;

        public event Action<Ray> OnMouseMove;
        public event Action<Ray> OnMouseClick;

        private void Awake() => _camera = GetComponent<Camera>();

        private void Update()
        {
            Ray lookRay = _camera.ScreenPointToRay(UnityEngine.Input.mousePosition);
            OnMouseMove?.Invoke(lookRay);

            if (UnityEngine.Input.GetMouseButtonDown(LeftMouseButtonIndex))
            {
                OnMouseClick?.Invoke(lookRay);
            }
        }
    }
}
