using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
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
        public Dictionary<PlayerNumber, List<WallGameObject>> PlayerWallsList { get; private set; }
        private int _lastFreeWallIndexInFirst = 0;
        private int _lastFreeWallIndexInSecond = 0;
        private bool _isInitialized = false;

        private Highlightable _pawnHighLight1;
        private Highlightable _pawnHighLight2;

        private bool _isHighlightedPawn = false;
        private bool _isHighlightedWalls = false;

        public void Start()
        {
            _coordsConverter = _view.CoordsConverter;
            _pawnHighLight1 = _pawn1.GetComponent<Highlightable>();
            _pawnHighLight2 = _pawn2.GetComponent<Highlightable>();
        }

        private void InitializePlayerWalls(PlayerInfoContainer<PlayerInfo> playerInfos)
        {
            PlayerWallsList = new Dictionary<PlayerNumber, List<WallGameObject>>
            {
                { PlayerNumber.First, new List<WallGameObject>() },
                { PlayerNumber.Second, new List<WallGameObject>() }
            };

            const float wallHeight = 0.915f;

            for (int i = 0; i < playerInfos.First.WallCount; i++)
            {
                var coords = new Vector3(_coordsConverter.CenterPoint.x - CoordsConverter.BoardCellHalfSize + i,
                                         wallHeight,
                                         _coordsConverter.CenterPoint.z - (CoordsConverter.BoardCellHalfSize + 1.0f));

                var wall = new WallGameObject(
                    Instantiate(_wallPrefab, coords, Quaternion.identity, _wallsParent.transform),
                    coords,
                    PlayerNumber.First);

                wall.GameObject.layer = GetWallLayer(PlayerNumber.First);
                PlayerWallsList[PlayerNumber.First].Add(wall);
            }

            for (int i = 0; i < playerInfos.Second.WallCount; i++)
            {
                var coords = new Vector3(_coordsConverter.CenterPoint.x - CoordsConverter.BoardCellHalfSize + i,
                                         wallHeight,
                                         _coordsConverter.CenterPoint.z + (CoordsConverter.BoardCellHalfSize + 1.0f));

                var wall = new WallGameObject(
                    Instantiate(_wallPrefab, coords, Quaternion.identity, _wallsParent.transform),
                    coords,
                    PlayerNumber.Second);

                wall.GameObject.layer = GetWallLayer(PlayerNumber.Second);
                PlayerWallsList[PlayerNumber.Second].Add(wall);
            }
        }

        public GameObject GetPawn(PlayerNumber playerNumber) => playerNumber switch
        {
            PlayerNumber.First => _pawn1,
            PlayerNumber.Second => _pawn2,
            _ => throw new ArgumentOutOfRangeException()
        };

        public GameObject GetWall(PlayerNumber playerNumber) => PlayerWallsList[playerNumber]
                                                                .First(wall => wall.AtStartPosition)
                                                                .GameObject;

        private Highlightable GetPawnHighlight(PlayerNumber playerNumber) => playerNumber switch
        {
            PlayerNumber.First => _pawnHighLight1,
            PlayerNumber.Second => _pawnHighLight2,
            _ => throw new ArgumentOutOfRangeException()
        };

        public void MovePawn(PlayerNumber playerNumber, Coords newCoords, [NotNull] Action finHandler)
        {
            // todo: add animations
            Transform pawnTransform = GetPawn(playerNumber).transform;

            _animations.Move(pawnTransform, pawnTransform.position, _coordsConverter.ToVector3(newCoords), finHandler);
        }

        public void PlaceWall(PlayerInfoContainer<PlayerInfo> playerInfos, PlayerNumber playerNumber, WallCoords newCoords)
        {
            Quaternion quaternion = newCoords.Rotation switch
            {
                WallRotation.Horizontal => Quaternion.AngleAxis(90f, Vector3.up),
                WallRotation.Vertical => Quaternion.identity,
                _ => throw new ArgumentOutOfRangeException()
            };

            int lastFreeWallIndex = playerNumber switch
            {
                PlayerNumber.First => _lastFreeWallIndexInFirst++,
                PlayerNumber.Second => _lastFreeWallIndexInSecond++,
                _ => throw new ArgumentOutOfRangeException()
            };

            PlayerWallsList[playerNumber][lastFreeWallIndex]
                .PlaceWallGameObject(_coordsConverter.ToVector3(newCoords), quaternion);
        }

        public void ResetWallsPosition(PlayerInfoContainer<PlayerInfo> playerInfos)
        {
            if (_isInitialized == false)
            {
                InitializePlayerWalls(playerInfos);
                _isInitialized = true;
            }

            PlayerWallsList.Values.ToList().ForEach(walls => walls.ForEach(wall => wall.ResetToStartPosition()));

            _lastFreeWallIndexInFirst = 0;
            _lastFreeWallIndexInSecond = 0;
        }

        public bool TryChangePawnHighlight(PlayerNumber playerNumber, bool highlighted)
        {
            if (_isHighlightedPawn == highlighted)
            {
                return false;
            }

            GetPawnHighlight(playerNumber).Change(highlighted);
            _isHighlightedPawn = highlighted;

            return true;
        }

        public bool TryChangeWallsHighlight(PlayerNumber playerNumber, bool highlighted)
        {
            if (_isHighlightedWalls == highlighted)
            {
                return false;
            }

            foreach (WallGameObject wall in PlayerWallsList[playerNumber].Where(wall => wall.AtStartPosition))
            {
                wall.Highlightable.Change(highlighted);
            }

            _isHighlightedWalls = highlighted;

            return true;
        }

        public void TurnOffAllHighlight(PlayerNumber playerNumber)
        {
            TryChangePawnHighlight(playerNumber, false);
            TryChangeWallsHighlight(playerNumber, false);
        }

        public static int GetWallLayer(PlayerNumber playerNumber) => playerNumber switch
        {
            PlayerNumber.First => LayerMask.NameToLayer("FirstPlayersWall"),
            PlayerNumber.Second => LayerMask.NameToLayer("SecondPlayersWall"),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}
