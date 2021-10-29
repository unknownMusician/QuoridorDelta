using QuoridorDelta.View.Proxy;
using UnityEngine;

namespace QuoridorDelta.View
{
    public sealed class Initializer : MonoBehaviour
    {
        [SerializeField] private UnityView _view;
        [SerializeField] private UnityInput _input;
        [SerializeField] private UnityProxy _proxy;

        public void Start() => _proxy.StartGame(_view, _input);
    }
}
