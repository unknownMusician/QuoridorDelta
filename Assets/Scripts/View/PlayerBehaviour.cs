using QuoridorDelta.Model;
using System;
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

        private GameObject GetPawn(PlayerType playerType) => playerType switch
        {
            PlayerType.First => _pawn1,
            PlayerType.Second => _pawn2,
            _ => throw new ArgumentOutOfRangeException()
        };

        public void MovePawn(PlayerType playerType, Coords newCoords)
        {
            // todo: add animations
            GetPawn(playerType).transform.position = _coordsConverter.ToVector3(newCoords);
        }

        public void PlaceWall(PlayerType playerType, WallCoords newCoords)
        {
            // todo: add animations
            Quaternion quaternion = newCoords.Orientation switch
            {
                WallOrientation.Horizontal => Quaternion.AngleAxis(90f, Vector3.up),
                WallOrientation.Vertical => Quaternion.identity,
                _ => throw new ArgumentOutOfRangeException()
            };

            Instantiate(_wallPrefab, _coordsConverter.ToVector3(newCoords), quaternion, _wallsParent.transform);
        }
    }
}