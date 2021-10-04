using QuoridorDelta.Model;
using QuoridorDelta.View.Proxy;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QuoridorDelta.View
{
    [RequireComponent(typeof(Camera))]
    public sealed class View : MonoBehaviour, ISyncView
    {
        [SerializeField] private UnityProxy _proxy;
        [SerializeField] private UserInput _input;
        [SerializeField] private LayerMask _layerMask;
        [SerializeField] private GameObject _boardObject;
        [SerializeField] private GameObject _backlightCellPrefab;
        [SerializeField] private Transform _backlightsParent;

        [Header("UI")] [SerializeField] private GameObject _moveTypeChoiceMenu;
        [SerializeField] private GameObject _gameTypeChoiceMenu;
        [SerializeField] private GameObject _wrongMoveInfoMenu;
        [SerializeField] private GameObject _winnerInfoMenu;
        [SerializeField] private GameObject _restartBlock;


        public CoordsConverter CoordsConverter { get; private set; }
        private PlayerBehaviour _pawnBehaviour;
        private RaycastToDesk _raycastToDesk;
        private Camera _camera;
        private Backlight _backlight;
        private const int _maxWallCount = 10;


        private Action<MoveType> _moveTypeHandler;
        private Action<Coords> _movePawnHandler;
        private Action<WallCoords> _placeWallHandler;
        private Action<GameType> _getGameType;
        private Action<bool> _shouldRestart;

        private void Awake()
        {
            _camera = GetComponent<Camera>();
            _pawnBehaviour = GetComponent<PlayerBehaviour>();
            CoordsConverter = new CoordsConverter(_boardObject.transform.position);
            _raycastToDesk = new RaycastToDesk(_camera, _layerMask, 100f, CoordsConverter);
            _backlight = new Backlight(CoordsConverter, _backlightCellPrefab, _backlightsParent);
        }

        private void Start() => _proxy.StartGame(this);

        public void GetMoveType(PlayerType playerType, Action<MoveType> handler)
        {
            _moveTypeChoiceMenu.SetActive(true);
            _moveTypeHandler = handler;
        }

        public void GetMovePawnCoords(PlayerType playerType, IEnumerable<Coords> possibleMoves, Action<Coords> handler)
        {
            _movePawnHandler = handler;
            _backlight.TurnOnLightOnCells(possibleMoves);
            _input.OnLeftMouseButtonClicked += PawnCoordsClickHandler;
        }

        public void GetPlaceWallCoords(PlayerType playerType, IEnumerable<WallCoords> possibleMoves, Action<WallCoords> handler)
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
                _moveTypeChoiceMenu.SetActive(false);
            }
        }

        public void PlaceWallButtonClick()
        {
            if (_moveTypeHandler != null)
            {
                _moveTypeHandler(MoveType.PlaceWall);
                _moveTypeHandler = null;
                _moveTypeChoiceMenu.SetActive(false);
            }
        }

        private void SendMovePawnCoords(Coords coords)
        {
            if (_movePawnHandler != null)
            {
                _movePawnHandler(coords);
                _movePawnHandler = null;
                _backlight.TurnOffLights();
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
        }

        private void WallCoordsClickHandler()
        {
            if (_raycastToDesk.TryGetPlaceWallCoords(out WallCoords coords))
            {
                _input.OnLeftMouseButtonClicked -= WallCoordsClickHandler;
                SendPlaceWallCoords(coords);
            }
        }

        public void MovePawn(PlayerType playerType, Coords newCoords) => _pawnBehaviour.MovePawn(playerType, newCoords);

        public void PlaceWall(PlayerType playerType, WallCoords newCoords) =>
            _pawnBehaviour.PlaceWall(playerType, newCoords);

        public void GetGameType(Action<GameType> handler)
        {
            _getGameType = handler;
            _gameTypeChoiceMenu.SetActive(true);
        }

        public void ShowWrongMove(PlayerType playerType, MoveType moveType)
        {
            const float waitingTime = 2.0f;

            _wrongMoveInfoMenu.SetActive(true);
            StartCoroutine(Waiting(waitingTime, () => _wrongMoveInfoMenu.SetActive(false)));
        }

        public void ShowWinner(PlayerType playerType) => _winnerInfoMenu.SetActive(true);

        public void ShouldRestart(Action<bool> handler)
        {
            _shouldRestart = handler;
            _restartBlock.SetActive(true);
        }

        public void SetGameTypePvP()
        {
            _getGameType(GameType.PlayerVersusPlayer);
            _getGameType = null;
        }

        public void SetGameTypePvBot()
        {
            _getGameType(GameType.PlayerVersusBot);
            _getGameType = null;
        }

        public void Restart()
        {
            _shouldRestart(true);
            _shouldRestart = null;
        }

        public void Exit()
        {
            _shouldRestart(false);
            _shouldRestart = null;

            Application.Quit();
        }

        private IEnumerator Waiting(float time, Action onFin)
        {
            float t = 0.0f;

            while (t < 1.0f)
            {
                t += Time.deltaTime / time;

                yield return null;
            }

            onFin.Invoke();
        }
    }
}