using System;
using JetBrains.Annotations;
using UnityEngine;

namespace QuoridorDelta.View
{
    public sealed class ShouldRestartWindow : MonoBehaviour
    {
        [SerializeField] private GameObject _windowUI;
        private Action<bool> _handler;

        public void Show([NotNull] Action<bool> handler)
        {
            _handler = handler;
            _windowUI.SetActive(true);
        }

        public void Restart()
        {
            _handler(true);
        }

        public void Exit()
        {
            _handler(false);

            Application.Quit();
        }
    }
}
