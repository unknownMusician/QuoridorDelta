using System;
using System.Collections.Generic;
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
        private bool IsInitialized = false;
<<<<<<< Updated upstream
        private Highlightable _pawn1HighLight;
        private Highlightable _pawn2HighLight;
=======
        private Highlitable _pawn1HighLight;
        private Highlitable _pawn2HighLight;
        //private bool _isHighlightedPawn1;
        //private bool _isHighlightedPawn2;
        private bool _isHighlightedPawn = false;
        private bool _isHighlightedWalls = false;
>>>>>>> Stashed changes

        public void Start()
        {
            _coordsConverter = _view.CoordsConverter;
            _pawn1HighLight = _pawn1.GetComponent<Highlightable>();
            _pawn2HighLight = _pawn2.GetComponent<Highlightable>();
        }

        private void InitializePlayerWalls(PlayerInfos playerInfos)
        {
            PlayerWallsList = new Dictionary<PlayerNumber, List<WallGameObject>>();
            PlayerWallsList.Add(PlayerNumber.First, new List<WallGameObject>());
            PlayerWallsList.Add(PlayerNumber.Second, new List<WallGameObject>());

            for (int i = 0; i < playerInfos.First.WallCount; i++)
            {
                Vector3 coords = new Vector3(_coordsConverter.CenterPoint.x - 4.5f + i,
                                             0.915f,
                                             _coordsConverter.CenterPoint.z - 5.5f);

                WallGameObject wall =
                    new WallGameObject(Instantiate(_wallPrefab, coords, Quaternion.identity, _wallsParent.transform),
                                       coords, PlayerNumber.First);
                wall.GameObject.layer = LayerMask.NameToLayer("FirstPlayersWall");
                PlayerWallsList[PlayerNumber.First].Add(wall);
            }

            for (int i = 0; i < playerInfos.Second.WallCount; i++)
            {
                Vector3 coords = new Vector3(_coordsConverter.CenterPoint.x - 4.5f + i,
                                             0.915f,
                                             _coordsConverter.CenterPoint.z + 5.5f);

                WallGameObject wall =
                    new WallGameObject(Instantiate(_wallPrefab, coords, Quaternion.identity, _wallsParent.transform),
                                       coords, PlayerNumber.Second);
                wall.GameObject.layer = LayerMask.NameToLayer("SecondPlayersWall");
                PlayerWallsList[PlayerNumber.Second].Add(wall);
            }
        }

        public GameObject GetPawn(PlayerNumber playerNumber) => playerNumber switch
        {
            PlayerNumber.First => _pawn1,
            PlayerNumber.Second => _pawn2,
            _ => throw new ArgumentOutOfRangeException()
        };
<<<<<<< Updated upstream

        private Highlightable GetPawnHighlight(PlayerNumber playerNumber) => playerNumber switch
        {
            PlayerNumber.First => _pawn1HighLight,
            PlayerNumber.Second => _pawn2HighLight,
            _ => throw new ArgumentOutOfRangeException()
        };
=======
>>>>>>> Stashed changes

        public void MovePawn(PlayerNumber playerNumber, Coords newCoords, [NotNull] Action finHandler)
        {
            // todo: add animations
            Transform pawnTransform = GetPawn(playerNumber).transform;

            _animations.Move(pawnTransform, pawnTransform.position, _coordsConverter.ToVector3(newCoords), finHandler);
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
                default: throw new ArgumentOutOfRangeException();
            }

            PlayerWallsList[playerNumber][lastFreeWallIndex]
                .PlaceWallGameObject(_coordsConverter.ToVector3(newCoords), quaternion);
        }

        public void ResetWallsPosition(PlayerInfos playerInfos)
        {
            if (IsInitialized == false)
            {
                InitializePlayerWalls(playerInfos);
                IsInitialized = true;
            }

            foreach (WallGameObject wall in PlayerWallsList[PlayerNumber.First])
            {
                wall.ResetToStartPosition();
            }

            foreach (WallGameObject wall in PlayerWallsList[PlayerNumber.Second])
            {
                wall.ResetToStartPosition();
            }

            _lastFreeWallIndexInFirst = 0;
            _lastFreeWallIndexInSecond = 0;
        }
<<<<<<< Updated upstream

        public void TurnOnPawnHighLight(PlayerNumber playerNumber) => GetPawnHighlight(playerNumber).Change(true);
        public void TurnOffPawnHighLight(PlayerNumber playerNumber) => GetPawnHighlight(playerNumber).Change(false);
=======
        public void TryChangePawnHighlight(PlayerNumber playerNumber, bool highlighted)
        {
            if (_isHighlightedPawn == highlighted)
            {
                return;
            }
            switch (playerNumber)
            {
                case PlayerNumber.First:
                    _pawn1HighLight.Change(highlighted);
                    _isHighlightedPawn = highlighted;
                    break;
                case PlayerNumber.Second:
                    _pawn2HighLight.Change(highlighted);
                    _isHighlightedPawn = highlighted;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void TryChangeWallsHighlight(PlayerNumber playerNumber, bool highlighted)
        {
            if (_isHighlightedWalls == highlighted)
            {
                return;
            }
            foreach (WallGameObject wall in PlayerWallsList[playerNumber])
            {
                if (wall.AtStartPosition == false)
                {
                    continue;
                }
                wall.Highlightable.Change(highlighted);
            }
            _isHighlightedWalls = highlighted;
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
>>>>>>> Stashed changes
    }
}
