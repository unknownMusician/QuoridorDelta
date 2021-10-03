﻿using QuoridorDelta.View;
using System;
using UnityEngine;

namespace QuoridorDelta.Model
{
    public sealed class PlayerBehaviour : MonoBehaviour
    {
        [SerializeField] private GameObject _pawn1;
        [SerializeField] private GameObject _pawn2;
        [SerializeField] private GameObject _wallPrefab;
        [SerializeField] private GameObject _wallsParent;
        [SerializeField] private View.View _view;

        private CoordsConverter _coordsConverter;

        public void Start() => _coordsConverter = _view.CoordsConverter;

        private GameObject GetPawn(PlayerType playerType)
        {
            switch (playerType)
            {
                case PlayerType.First:
                    return _pawn1;
                    break;
                case PlayerType.Second:
                    return _pawn2;
                    break;
                default:
                    throw new Exception("Pawn with this type is not defined");
                    break;
            }
        }

        public void MovePawn(PlayerType playerType, Coords newCoords)
        {
            // todo: add animations
            GetPawn(playerType).transform.position = _coordsConverter.ToVector3(newCoords);
        }
        public void PlaceWall(PlayerType playerType, WallCoords newCoords)
        {
            // todo: add animations
            Instantiate(_wallPrefab, _coordsConverter.ToVector3(newCoords), Quaternion.identity);
        }
    }
}