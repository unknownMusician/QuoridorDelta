using QuoridorDelta.Model;
using UnityEngine;

namespace QuoridorDelta.View
{
    public sealed class Field : MonoBehaviour
    {
        [SerializeField] private PlayerInfoHolder _playerInfoHolder1 = default!;
        [SerializeField] private PlayerInfoHolder _playerInfoHolder2 = default!;

        public PlayerInfoContainer<PlayerInfoHolder> PlayerInfoHolders { get; private set; }

        public void Awake()
            => PlayerInfoHolders = new PlayerInfoContainer<PlayerInfoHolder>(_playerInfoHolder1, _playerInfoHolder2);
    }
}
