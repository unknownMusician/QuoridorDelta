using QuoridorDelta.View.Proxy;
using UnityEngine;

namespace QuoridorDelta.View.Refactor
{
    public sealed class Initializer : MonoBehaviour
    {
        [SerializeField] private View _view;
        [SerializeField] private Input _input;
        [SerializeField] private UnityProxy _proxy;

        public void Start() => _proxy.StartGame(_view, _input);
    }
}
