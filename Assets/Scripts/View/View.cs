using QuoridorDelta.Model;
using QuoridorDelta.View.Proxy;
using System;
using UnityEngine;

namespace QuoridorDelta.View
{
    [RequireComponent(typeof(Camera))]
    public sealed class View : MonoBehaviour, ISyncView
    {
        [SerializeField] private UnityProxy _proxy;
        [SerializeField] private GameObject _moveTypeChoiseMenu;
        [SerializeField] private UserInput _input;
        [SerializeField] private LayerMask _layerMask;
        private RaycastToDesk _raycastToDesk;
        private Camera _camera;


        private Action<MoveType> _moveTypeHandler;
        private Action<Coords> _movePawnHandler;
        private Action<WallCoords> _placeWallHandler;

        private void Start()
        {
            _proxy.StartGame(this);
            _camera = GetComponent<Camera>();
            _raycastToDesk = new RaycastToDesk(_camera, _layerMask);
        }

        public void GetMoveType(PlayerType playerType, Action<MoveType> handler)
        {
            _moveTypeChoiseMenu.SetActive(true);
            _moveTypeHandler = handler;
        }

        public void GetMovePawnCoords(PlayerType playerType, Action<Coords> handler)
        {
            _movePawnHandler = handler;
            _input.OnLeftMouseButtonClicked += PawnCoordsClickHandler;
        }

        public void GetPlaceWallCoords(PlayerType playerType, Action<WallCoords> handler)
        {
            _placeWallHandler = handler;
            _input.OnLeftMouseButtonClicked += WallCoordsClickHandler;
        }

        public void MovePawnButtonClick()
        {
            if (_moveTypeHandler != null)
            {
                _moveTypeHandler(MoveType.MovePawn);
                _moveTypeHandler = null;
                _moveTypeChoiseMenu.SetActive(false);
            }
        }
        public void PlaceWallButtonClick()
        {
            if (_moveTypeHandler != null)
            {
                _moveTypeHandler(MoveType.PlaceWall);
                _moveTypeHandler = null;
                _moveTypeChoiseMenu.SetActive(false);
            }
        }
        private void SendMovePawnCoords(Coords coords)
        {
            if (_movePawnHandler != null)
            {
                _movePawnHandler(coords);
                _movePawnHandler = null;
                Debug.Log($"MovePawn");
            }
        }
        private void SendPlaceWallCoords(WallCoords coords)
        {
            if (_placeWallHandler != null)
            {
                _placeWallHandler(coords);
                _placeWallHandler = null;
                Debug.Log($"PlaceWall");
            }
        }

        private void PawnCoordsClickHandler()
        {
            if (_raycastToDesk.TryGetPawnMoveCoords(out Coords coords))
            {
                _input.OnLeftMouseButtonClicked -= PawnCoordsClickHandler;
                SendMovePawnCoords(coords);
            }
            //else
            //{
            //    Debug.Log($"no");
            //}
        }
        private void WallCoordsClickHandler()
        {
            if (_raycastToDesk.TryGetPlaceWallCoords(out WallCoords coords))
            {
                _input.OnLeftMouseButtonClicked -= WallCoordsClickHandler;
                SendPlaceWallCoords(coords);
            }
        }
    }
}