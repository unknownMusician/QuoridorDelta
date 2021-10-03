using UnityEngine;
using System;

namespace QuoridorDelta.View
{
    public sealed class UserInput : MonoBehaviour
    {
        public event Action OnLeftMouseButtonClicked;

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                OnLeftMouseButtonClicked?.Invoke();
            }
        }
    }
}