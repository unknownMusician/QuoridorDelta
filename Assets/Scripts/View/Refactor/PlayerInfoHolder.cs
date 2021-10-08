using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using QuoridorDelta.Controller;
using QuoridorDelta.Model;
using UnityEngine;

namespace QuoridorDelta.View.Refactor
{
    public sealed class PlayerInfoHolder : MonoBehaviour
    {
        private const float MaxRaycastDistance = 100.0f;

        [SerializeField] private GameObject _pawn;
        [SerializeField] private GameObject[] _walls;
        [SerializeField] private LayerMask _boardMask;
        [SerializeField] private Vector3 _storedWallsStartPosition;

        [SerializeField] private MouseInputHandler _mouseHandler;
        [SerializeField] private CoordsConverter _converter;
        [SerializeField] private SoundKeeper _soundKeeper;

        [SerializeField] private float _pursueLerp;
        [SerializeField] private float _animationMoveTime;

        // todo

        private List<GameObject> _storedWalls;
        private Action<MoveType> _moveTypeHandler;
        private Action<Coords> _coordsHandler;
        private Action<WallCoords> _wallCoordsHandler;

        private GameObject _lastClickedObject;

        private Coords _lastPursueCoords;
        private WallCoords _lastPursueWallCoords;

        private void Awake() => _storedWalls = new List<GameObject>(_walls);

        public void GetMoveType([NotNull] Action<MoveType> handler)
        {
            _moveTypeHandler = handler;

            ChangeHighlighting(true);

            _mouseHandler.OnMouseClick += RaycastMoveType;
        }

        private void ChangeHighlighting(bool highlighted)
        {
            _storedWalls.ForEach(wall => wall.GetComponent<Highlightable>().Change(highlighted));
            _pawn.GetComponent<Highlightable>().Change(highlighted);
        }

        public void GetMovePawnCoords([NotNull] Action<Coords> handler)
        {
            _coordsHandler = handler;

            // todo
            _lastClickedObject = _pawn;

            _mouseHandler.OnMouseMove += PursueCursorPawn;
            _mouseHandler.OnMouseClick += RaycastGetCoords;
        }

        public void PursueCursorPawn(Ray ray)
        {
            if (!Physics.Raycast(ray, out RaycastHit hit, MaxRaycastDistance, _boardMask))
            {
                return;
            }

            Coords newPursueCoords = _converter.ToCoords(hit.point);

            if (newPursueCoords != _lastPursueCoords)
            {
                _soundKeeper.MagnetSound.PlayNext();
                _lastPursueCoords = newPursueCoords;
            }

            _lastClickedObject.transform.position = Vector3.Lerp(_lastClickedObject.transform.position,
                                                                 _converter.ToVector3(newPursueCoords),
                                                                 _pursueLerp);
        }

        private void PursueCursorWall(Ray ray)
        {
            if (!Physics.Raycast(ray, out RaycastHit hit, MaxRaycastDistance, _boardMask))
            {
                return;
            }

            WallCoords newPursueWallCoords = _converter.ToWallCoords(hit.point);

            if (newPursueWallCoords != _lastPursueWallCoords)
            {
                _soundKeeper.MagnetSound.PlayNext();
                _lastPursueWallCoords = newPursueWallCoords;
            }

            _lastClickedObject.transform.position = Vector3.Lerp(_lastClickedObject.transform.position,
                                                                 _converter.ToVector3(newPursueWallCoords),
                                                                 _pursueLerp);

            _lastClickedObject.transform.rotation = Quaternion.Slerp(_lastClickedObject.transform.rotation,
                                                                     _converter.GetWallQuaternion(newPursueWallCoords.Rotation),
                                                                     _pursueLerp);
        }

        public void GetPlaceWallCoords([NotNull] Action<WallCoords> handler)
        {
            _wallCoordsHandler = handler;
            _mouseHandler.OnMouseMove += PursueCursorWall;
            _mouseHandler.OnMouseClick += RaycastGetWallCoords;
        }

        private void RaycastMoveType(Ray ray)
        {
            if (!Physics.Raycast(ray, out RaycastHit hit, MaxRaycastDistance))
            {
                return;
            }

            GameObject hitObject = hit.collider.gameObject;

            if (hitObject == _pawn)
            {
                ChangeHighlighting(false);
                HandleRaycastMoveType(hitObject, MoveType.MovePawn);
            }
            else if (_storedWalls.Contains(hitObject))
            {
                ChangeHighlighting(false);
                HandleRaycastMoveType(hitObject, MoveType.PlaceWall);
            }
        }

        private void HandleRaycastMoveType(GameObject lastClickedObject, MoveType moveType)
        {
            _lastClickedObject = lastClickedObject;
            _mouseHandler.OnMouseMove -= PursueCursorPawn;
            _mouseHandler.OnMouseClick -= RaycastMoveType;

            _moveTypeHandler(moveType);
        }

        private void RaycastGetCoords(Ray ray)
        {
            if (!Physics.Raycast(ray, out RaycastHit hit, MaxRaycastDistance, _boardMask))
            {
                return;
            }

            _mouseHandler.OnMouseMove -= PursueCursorPawn;
            _mouseHandler.OnMouseClick -= RaycastGetCoords;

            _coordsHandler(_converter.ToCoords(hit.point));
        }

        private void RaycastGetWallCoords(Ray ray)
        {
            if (!Physics.Raycast(ray, out RaycastHit hit, MaxRaycastDistance, _boardMask))
            {
                return;
            }

            _mouseHandler.OnMouseMove -= PursueCursorWall;
            _mouseHandler.OnMouseClick -= RaycastGetWallCoords;

            _wallCoordsHandler(_converter.ToWallCoords(hit.point));
        }

        public void MovePawn(Coords newCoords)
        {
            // todo
            _lastClickedObject = _pawn;

            Move(_lastClickedObject.transform,
                 _converter.ToVector3(newCoords),
                 Quaternion.identity,
                 _soundKeeper.PawnMoveSound.PlayNext);
        }

        public void PlaceWall(WallCoords newCoords)
        {
            // todo
            if (!_storedWalls.Contains(_lastClickedObject))
            {
                _lastClickedObject = _storedWalls[0];
            }

            _storedWalls.Remove(_lastClickedObject);

            Move(_lastClickedObject.transform,
                 _converter.ToVector3(newCoords),
                 _converter.GetWallQuaternion(newCoords.Rotation),
                 _soundKeeper.WallMoveSound.PlayNext);
        }

        private void Move(
            [NotNull] Transform movable,
            Vector3 finPosition,
            Quaternion finRotation,
            [NotNull] Action endHandler
        )
        {
            Vector3 startPosition = movable.position;
            Quaternion startRotation = movable.rotation;

            void Animation(float t)
            {
                movable.position = Vector3.Lerp(startPosition, finPosition, t);
                movable.rotation = Quaternion.Slerp(startRotation, finRotation, t);
            }

            StartCoroutine(Animations.Lerp(_animationMoveTime, Animation, endHandler));
        }

        public void ResetElements(Coords pawnPosition)
        {
            _storedWalls = new List<GameObject>(_walls);
            _pawn.transform.SetPositionAndRotation(_converter.ToVector3(pawnPosition), Quaternion.identity);

            for (int i = 0; i < _storedWalls.Count; i++)
            {
                _storedWalls[i]
                    .transform.SetPositionAndRotation(_storedWallsStartPosition + Vector3.right * i,
                                                      Quaternion.identity);
            }
        }
    }
}
