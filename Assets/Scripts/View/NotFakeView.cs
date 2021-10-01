using QuoridorDelta.Model;
using QuoridorDelta.View.Proxy;
using System;
using UnityEngine;

namespace QuoridorDelta.View
{
    // todo: remove
    public class NotFakeView : MonoBehaviour, ISyncView
    {
        [SerializeField] private RaycastToDesk _raycastToDesk;
        [SerializeField] private UnityProxy _proxy;

        private Action<MoveType> _moveTypeHandler;
        private Action<Coords> _movePawnHandler;
        private Action<WallCoords> _placeWallHandler;

        private void Start() => _proxy.StartGame(this);

        public void GetMoveType(PlayerType playerType, Action<MoveType> handler)
        {
            //if ()
            //{

            //}
            _moveTypeHandler = handler;
        }

        public void GetMovePawnCoords(PlayerType playerType, Action<Coords> handler)
        {
            _movePawnHandler = handler;

            //handler(_raycastToDesk.MakeRaycast());
        }

        public void GetPlaceWallCoords(PlayerType playerType, Action<WallCoords> handler)
        {
            _placeWallHandler = handler;

            handler(((9, 15), WallOrientation.Vertical));
        }

        public void MovePawnButtonClick()
        {
            if (_moveTypeHandler != null)
            {
                _moveTypeHandler(MoveType.MovePawn);
                Debug.Log($"MovePawn");
                _moveTypeHandler = null;
            }
        }

        public void PlaceWallButtonClick()
        {
            Debug.Log($"PlaceWall");
            _moveTypeHandler(MoveType.PlaceWall);
        }

    }
}