using QuoridorDelta.View.Proxy;
using UnityEngine;

namespace QuoridorDelta.View
{
    public sealed class Initializer : MonoBehaviour
    {
        [SerializeField] private UnityView _view = default!;
        [SerializeField] private UnityInput _input = default!;
        [SerializeField] private UnityProxy _proxy = default!;

        public void Start() => _proxy!.StartGame(_view!, _input!);
    }
}
