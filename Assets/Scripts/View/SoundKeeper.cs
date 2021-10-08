using UnityEngine;

namespace QuoridorDelta.View
{
    public sealed class SoundKeeper : MonoBehaviour
    {
        [SerializeField] private RandomSoundPlayer _pawnMoveSound;
        [SerializeField] private RandomSoundPlayer _wallMoveSound;
        [SerializeField] private RandomSoundPlayer _magnetSound;

        public RandomSoundPlayer PawnMoveSound => _pawnMoveSound;
        public RandomSoundPlayer WallMoveSound => _wallMoveSound;
        public RandomSoundPlayer MagnetSound => _magnetSound;
    }
}
