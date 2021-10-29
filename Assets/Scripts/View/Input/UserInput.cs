using UnityEngine;
using System;
using QuoridorDelta.Model;

namespace QuoridorDelta.View
{
    public sealed class UserInput : MonoBehaviour
    {
        private const int LeftMouseButtonIndex = 0;

        public event Action OnLeftMouseButtonClicked;
        public event Action<PlayerNumber> OnLeftMouseButtonClicked2;
        public event Action<PlayerNumber> OnMouseHovering;

        public PlayerNumber UserType;

        public bool IsMouseHoverActive { get; set; } = false;

        private void Update()
        {
            if (Input.GetMouseButtonDown(UserInput.LeftMouseButtonIndex))
            {
                OnLeftMouseButtonClicked?.Invoke();
                OnLeftMouseButtonClicked2?.Invoke(UserType);
            }
            if (IsMouseHoverActive)
            {
                OnMouseHovering?.Invoke(UserType);
            }
        }
    }
}
