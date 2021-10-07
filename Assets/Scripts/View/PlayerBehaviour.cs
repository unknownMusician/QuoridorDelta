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

        private Highlightable _pawnHighLight1;
        private Highlightable _pawnHighLight2;
        private Ghostable _pawnGhost1;
        private Ghostable _pawnGhost2;

        private bool _isHighlightedPawn = false;
        private bool _isHighlightedWalls = false;
        private bool _isGhostedPawn = false;
        private bool _isGhostedWalls = false;

        public void Start()
        {
            _coordsConverter = _view.CoordsConverter;
            _pawnHighLight1 = _pawn1.GetComponent<Highlightable>();
            _pawnHighLight2 = _pawn2.GetComponent<Highlightable>();
            _pawnGhost1 = _pawn1.GetComponent<Ghostable>();
            _pawnGhost2 = _pawn2.GetComponent<Ghostable>();
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
        public GameObject GetWall(PlayerNumber playerNumber)
        {
            foreach (WallGameObject wall in PlayerWallsList[playerNumber])
            {
                if (wall.AtStartPosition)
                {
                    return wall.GameObject;
                }
            }
            throw new Exception("All walls putted on board");
        }

        private Highlightable GetPawnHighlight(PlayerNumber playerNumber) => playerNumber switch
        {
            PlayerNumber.First => _pawnHighLight1,
            PlayerNumber.Second => _pawnHighLight2,
            _ => throw new ArgumentOutOfRangeException()
        };
        private Ghostable GetPawnGhost(PlayerNumber playerNumber) => playerNumber switch
        {
            PlayerNumber.First => _pawnGhost1,
            PlayerNumber.Second => _pawnGhost2,
            _ => throw new ArgumentOutOfRangeException()
        };

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
        public bool TryChangePawnGhost(PlayerNumber playerNumber, bool ghosted)
        {
            if (_isGhostedPawn == ghosted)
            {
                return false;
            }
            GetPawnGhost(playerNumber).Change(ghosted);
            _isGhostedPawn = ghosted;
            return true;
        }
        public bool TryChangeWallsHighlight(PlayerNumber playerNumber, bool highlighted)
        {
            if (_isHighlightedWalls == highlighted)
            {
                return false;
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
            return true;
        }
        public bool TryChangeWallsGhost(PlayerNumber playerNumber, bool ghosted)
        {
            if (_isGhostedWalls == ghosted)
            {
                return false;
            }
            foreach (WallGameObject wall in PlayerWallsList[playerNumber])
            {
                if (wall.AtStartPosition == false)
                {
                    continue;
                }
                wall.Highlightable.Change(ghosted);
            }
            _isGhostedWalls = ghosted;
            return true;
        }

        public void TurnOffAllHighlight(PlayerNumber playerNumber)
        {
            TryChangePawnHighlight(playerNumber, false);
            TryChangeWallsHighlight(playerNumber, false);
        }
        public void TurnOffAllGhost()
        {
            TryChangePawnGhost(PlayerNumber.First, false);
            TryChangeWallsGhost(PlayerNumber.First, false);
            TryChangePawnGhost(PlayerNumber.Second, false);
            TryChangeWallsGhost(PlayerNumber.Second, false);
        }

        public static int GetWallLayer(PlayerNumber playerNumber) => playerNumber switch
        {
            PlayerNumber.First => LayerMask.NameToLayer("FirstPlayersWall"),
            PlayerNumber.Second => LayerMask.NameToLayer("SecondPlayersWall"),
            _ => throw new ArgumentOutOfRangeException()
        };
        //public static void SetAlpha(GameObject gameObject, float alpha)
        //{
        //    Color color = gameObject.GetComponentInChildren<Renderer>().material.color;
        //    color.a = alpha;
        //}
    }
}
