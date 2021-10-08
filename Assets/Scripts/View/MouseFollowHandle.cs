using QuoridorDelta.Model;
using System;
using UnityEngine;

namespace QuoridorDelta.View
{
    public sealed class MouseFollowHandle : MonoBehaviour
    {
        public event Action<PlayerNumber> OnMouseFollowing;

        public PlayerNumber CurrentPlayer { private get; set; }

        private void Update() => OnMouseFollowing?.Invoke(CurrentPlayer);
    }
}
