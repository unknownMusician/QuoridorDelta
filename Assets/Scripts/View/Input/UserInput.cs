using UnityEngine;
using System;

namespace QuoridorDelta.View
{
    public sealed class UserInput : MonoBehaviour
    {
        private const int LeftMouseButtonIndex = 0;
        public event Action OnLeftMouseButtonClicked;

        private void Update()
        {
            if (Input.GetMouseButtonDown(LeftMouseButtonIndex))
            {
                OnLeftMouseButtonClicked?.Invoke();
            }
        }
    }
}
