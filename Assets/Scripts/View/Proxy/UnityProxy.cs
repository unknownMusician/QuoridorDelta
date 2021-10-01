using System.Collections;
using UnityEngine;

namespace QuoridorDelta.View.Proxy
{
    public sealed class UnityProxy : MonoBehaviour
    {
        // todo
        //[SerializeField] private FakeView _fakeView;

        private bool _isAlive = true;
        private QuoridorProxy _proxy;
        private ISyncView _view;

        private void OnDestroy()
        {
            _isAlive = false;

            if (_proxy != null)
            {
                _proxy.Dispose();
                _proxy = null;
            }
        }
        // todo
        //private void Start() => StartGame(_fakeView);

        public void StartGame(ISyncView view)
        {
            _view = view;
            _proxy = new QuoridorProxy();
            StartCoroutine(Listening());
        }

        private IEnumerator Listening()
        {
            while (_isAlive)
            {
                // todo
                if (_proxy.MoveTypeRequest != null && !_proxy.MoveTypeRequest.Initialized)
                {
                    _view.GetMoveType(_proxy.MoveTypeRequest.Input, _proxy.MoveTypeRequest.Initialize);
                    Debug.Log($"Initialized MoveTypeRequest");
                }
                else if (_proxy.MovePawnRequest != null && !_proxy.MovePawnRequest.Initialized)
                {
                    _view.GetMovePawnCoords(_proxy.MovePawnRequest.Input, _proxy.MovePawnRequest.Initialize);
                    Debug.Log($"Initialized MovePawnRequest");
                }
                else if (_proxy.PlaceWallRequest != null && !_proxy.PlaceWallRequest.Initialized)
                {
                    _view.GetPlaceWallCoords(_proxy.PlaceWallRequest.Input, _proxy.PlaceWallRequest.Initialize);
                    Debug.Log($"Initialized PlaceWallRequest");
                }

                yield return null;
            }
        }
    }
}