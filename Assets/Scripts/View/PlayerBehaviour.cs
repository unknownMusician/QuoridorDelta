using System;
<<<<<<< HEAD
using System.Collections.Generic;
=======
using JetBrains.Annotations;
>>>>>>> 43b2059e33688bb65dfb4b6dfc51ff028b677ed0
using QuoridorDelta.Model;
using UnityEngine;

namespace QuoridorDelta.View
{
    public sealed class PlayerBehaviour : MonoBehaviour
    {
        [SerializeField] private GameObject _pawn1;
        [SerializeField] private GameObject _pawn2;
        [SerializeField] private GameObject _wallPrefab;
        [SerializeField] private GameObject _wallsParent;
        [SerializeField] private View _view;
        [SerializeField] private Animations _animations;


        private CoordsConverter _coordsConverter;
        private Dictionary<PlayerInfo, List<WallGameObject>> _playerWallsList;
        //private readonly Vector3 _firstWall = new Vector3(-4.5f, 0.915f, -5.5f);

        private int _lastFreeWallIndexInFirst = 0;
        private int _lastFreeWallIndexInSecond = 0;

        private bool IsInitialized = false;

        public void Start()
        {
            _coordsConverter = _view.CoordsConverter;
            
        }


        private void InitializePlayerWalls(PlayerInfos playerInfos)
        {
            _playerWallsList = new Dictionary<PlayerInfo, List<WallGameObject>>();
            _playerWallsList[playerInfos.First] = new List<WallGameObject>();
            _playerWallsList[playerInfos.Second] = new List<WallGameObject>();
            for (int i = 0; i < playerInfos.First.WallCount; i++)
            {
                Vector3 coords = new Vector3(_coordsConverter.CenterPoint.x - 4.5f + i, 0.915f, _coordsConverter.CenterPoint.z - 5.5f);
                WallGameObject wall = new WallGameObject(Instantiate(_wallPrefab, coords, Quaternion.identity, _wallsParent.transform), coords);
                _playerWallsList[playerInfos.First].Add(wall);
            }

            for (int i = 0; i < playerInfos.Second.WallCount; i++)
            {
                Vector3 coords = new Vector3(_coordsConverter.CenterPoint.x - 4.5f + i, 0.915f, _coordsConverter.CenterPoint.z + 5.5f);
                WallGameObject wall = new WallGameObject(Instantiate(_wallPrefab, coords, Quaternion.identity, _wallsParent.transform), coords);
                _playerWallsList[playerInfos.Second].Add(wall);
            }

            IsInitialized = true;
        }
        private GameObject GetPawn(PlayerNumber playerNumber) => playerNumber switch
        {
            PlayerNumber.First => _pawn1,
            PlayerNumber.Second => _pawn2,
            _ => throw new ArgumentOutOfRangeException()
        };

        public void MovePawn(PlayerNumber playerNumber, Coords newCoords, [NotNull] Action finHandler)
        {
            // todo: add animations
            Transform pawnTransform = GetPawn(playerNumber).transform;
            
            _animations.Move(pawnTransform, 
                             pawnTransform.position,
                             _coordsConverter.ToVector3(newCoords),
                             finHandler);
        }
        public void PlaceWall(PlayerInfos playerInfos, PlayerNumber playerNumber, WallCoords newCoords)
        {
            Quaternion quaternion = newCoords.Rotation switch
            {
                WallRotation.Horizontal => Quaternion.AngleAxis(90f, Vector3.up),
                WallRotation.Vertical => Quaternion.identity,
                _ => throw new ArgumentOutOfRangeException()
            };

            PlayerInfo playerInfo;
            int lastFreeWallIndex;
            switch (playerNumber)
            {
                case PlayerNumber.First:
                    playerInfo = playerInfos.First;
                    lastFreeWallIndex = _lastFreeWallIndexInFirst++;
                    break;
                case PlayerNumber.Second:
                    playerInfo = playerInfos.Second;
                    lastFreeWallIndex = _lastFreeWallIndexInSecond++;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            _playerWallsList[playerInfo][lastFreeWallIndex].PlaceWallGameObject(_coordsConverter.ToVector3(newCoords), quaternion);
        }
        public void ResetWallsPosition(PlayerInfos playerInfos)
        {
            if (IsInitialized == false)
            {
                InitializePlayerWalls(playerInfos);
            }
            foreach (WallGameObject wall in _playerWallsList[playerInfos.First])
            {
                wall.ResetToStartPosition();
            }
            foreach (WallGameObject wall in _playerWallsList[playerInfos.Second])
            {
                wall.ResetToStartPosition();
            }
        }
    }
}
