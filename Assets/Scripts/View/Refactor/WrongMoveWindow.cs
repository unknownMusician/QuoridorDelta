using UnityEngine;

namespace QuoridorDelta.View.Refactor
{
    public sealed class WrongMoveWindow : MonoBehaviour
    {
        [SerializeField] private GameObject _windowUI;
        [SerializeField] private float _delayTime;

        public void Show()
        {
            _windowUI.SetActive(true);

            StartCoroutine(Animations.Wait(_delayTime, () => _windowUI.SetActive(false)));
        }
    }
}
