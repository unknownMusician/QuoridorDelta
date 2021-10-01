using System;
using System.Collections;
using UnityEngine;

namespace QuoridorDelta.View.Proxy
{
    public sealed class UnityProxy : MonoBehaviour
    {
        // todo
        //[SerializeField] private FakeView _fakeView;

        private bool _startedMoveTypeInitialization = false;
        private bool _startedMovePawnInitialization = false;
        private bool _startedPlaceWallInitialization = false;

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
                if (_proxy.MoveTypeRequest != null && !_proxy.MoveTypeRequest.Initialized && !_startedMoveTypeInitialization)
                {
                    _startedMoveTypeInitialization = true;
                    _view.GetMoveType(_proxy.MoveTypeRequest.Input, handle => { _startedMoveTypeInitialization = false; _proxy.MoveTypeRequest.Initialize(handle); });
                    Debug.Log($"Initialized MoveTypeRequest");
                }
                else if (_proxy.MovePawnRequest != null && !_proxy.MovePawnRequest.Initialized && !_startedMovePawnInitialization)
                {
                    _startedMovePawnInitialization = true;
                    _view.GetMovePawnCoords(_proxy.MovePawnRequest.Input, handle => { _startedMovePawnInitialization = false; _proxy.MovePawnRequest.Initialize(handle); });
                    Debug.Log($"Initialized MovePawnRequest");
                }
                else if (_proxy.PlaceWallRequest != null && !_proxy.PlaceWallRequest.Initialized && !_startedPlaceWallInitialization)
                {
                    _startedMovePawnInitialization = true;
                    _view.GetPlaceWallCoords(_proxy.PlaceWallRequest.Input, handle => { _startedMovePawnInitialization = false; _proxy.PlaceWallRequest.Initialize(handle); });
                    Debug.Log($"Initialized PlaceWallRequest");
                }

                yield return null;
            }
        }
    }
}