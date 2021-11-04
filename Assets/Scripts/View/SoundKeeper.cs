using UnityEngine;

namespace QuoridorDelta.View
{
    public sealed class SoundKeeper : MonoBehaviour
    {
        [SerializeField] private RandomSoundPlayer _pawnMoveSound = default!;
        [SerializeField] private RandomSoundPlayer _wallMoveSound = default!;
        [SerializeField] private RandomSoundPlayer _magnetSound = default!;

        public RandomSoundPlayer PawnMoveSound => _pawnMoveSound;
        public RandomSoundPlayer WallMoveSound => _wallMoveSound;
        public RandomSoundPlayer MagnetSound => _magnetSound;
    }
}
