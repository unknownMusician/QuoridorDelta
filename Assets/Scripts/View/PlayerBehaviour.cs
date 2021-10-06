using System;
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

        private CoordsConverter _coordsConverter;

        public void Start() => _coordsConverter = _view.CoordsConverter;

        private GameObject GetPawn(PlayerNumber playerNumber) => playerNumber switch
        {
            PlayerNumber.First => _pawn1,
            PlayerNumber.Second => _pawn2,
            _ => throw new ArgumentOutOfRangeException()
        };

        public void MovePawn(PlayerNumber playerNumber, Coords newCoords)
        {
            // todo: add animations
            GetPawn(playerNumber).transform.position = _coordsConverter.ToVector3(newCoords);
        }

        public void PlaceWall(PlayerNumber playerNumber, WallCoords newCoords)
        {
            // todo: add animations
            Quaternion quaternion = newCoords.Rotation switch
            {
                WallRotation.Horizontal => Quaternion.AngleAxis(90f, Vector3.up),
                WallRotation.Vertical => Quaternion.identity,
                _ => throw new ArgumentOutOfRangeException()
            };

            Instantiate(_wallPrefab, _coordsConverter.ToVector3(newCoords), quaternion, _wallsParent.transform);
        }
    }
}
