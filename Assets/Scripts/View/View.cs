﻿using QuoridorDelta.Model;
using QuoridorDelta.View.Proxy;
using System;
using System.Collections.Generic;
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
        [SerializeField] private GameObject _boardObject;


        public CoordsConverter CoordsConverter { get; set; }
        private PlayerBehaviour _pawnBehaviour;
        private RaycastToDesk _raycastToDesk;
        private Camera _camera;


        private Action<MoveType> _moveTypeHandler;
        private Action<Coords> _movePawnHandler;
        private Action<WallCoords> _placeWallHandler;

        private void Start()
        {
            _proxy.StartGame(this);
            _camera = GetComponent<Camera>();
            _raycastToDesk = new RaycastToDesk(_camera, _layerMask, CoordsConverter);
            _pawnBehaviour = GetComponent<PlayerBehaviour>();
            CoordsConverter = new CoordsConverter(_boardObject.transform.position);
        }

        public void GetMoveType(PlayerType playerType, Action<MoveType> handler)
        {
            _moveTypeChoiseMenu.SetActive(true);
            _moveTypeHandler = handler;
        }

        public void GetMovePawnCoords(PlayerType playerType, IEnumerable<Coords> possibleMoves, Action<Coords> handler)
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
                //Debug.Log($"MovePawn");
            }
        }
        private void SendPlaceWallCoords(WallCoords coords)
        {
            if (_placeWallHandler != null)
            {
                _placeWallHandler(coords);
                _placeWallHandler = null;
                //Debug.Log($"PlaceWall");
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

        public void GetGameType(Action<GameType> handler)
        {
            throw new NotImplementedException();
        }

        public void MovePawn(PlayerType playerType, Coords newCoords) => _pawnBehaviour.MovePawn(playerType, newCoords);

        public void PlaceWall(PlayerType playerType, WallCoords newCoords) => _pawnBehaviour.PlaceWall(playerType, newCoords);

        public void ShowWrongMove(PlayerType playerType, MoveType moveType)
        {
            throw new NotImplementedException();
        }

        public void ShowWinner(PlayerType playerType)
        {
            throw new NotImplementedException();
        }

        public void ShouldRestart(Action<bool> handler)
        {
            throw new NotImplementedException();
        }
    }
}