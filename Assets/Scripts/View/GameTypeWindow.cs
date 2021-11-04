using System;
using QuoridorDelta.Controller;
using UnityEngine;

namespace QuoridorDelta.View
{
    public sealed class GameTypeWindow : MonoBehaviour
    {
        [SerializeField] private GameObject _windowUI = default!;

        private Action<GameType>? _handler;

        public void Show(Action<GameType> handler)
        {
            _handler = handler;
            _windowUI.SetActive(true);
        }

        public void ChoosePvP()
        {
            _handler!(GameType.VersusPlayer);
            _handler = null;
        }

        public void ChoosePvBot()
        {
            _handler!(GameType.VersusBot);
            _handler = null;
        }
    }
}
