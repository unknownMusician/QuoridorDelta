using UnityEngine;

namespace QuoridorDelta.View
{
    public sealed class WrongMoveWindow : MonoBehaviour
    {
        [SerializeField] private GameObject _windowUI = default!;
        [SerializeField] private float _delayTime;

        public void Show()
        {
            _windowUI.SetActive(true);

            StartCoroutine(Animations.Wait(_delayTime, () => _windowUI.SetActive(false)));
        }
    }
}
