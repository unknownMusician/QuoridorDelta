using QuoridorDelta.View.Proxy;
using System;
using System.Collections;
using System.Collections.Generic;
using QuoridorDelta.Controller;
using QuoridorDelta.Model;
using UnityEngine;

namespace QuoridorDelta.View
{
    [RequireComponent(typeof(Camera))]
    public sealed class View : MonoBehaviour, ISyncView, ISyncInput
    {
        [SerializeField] private UnityProxy _proxy;
        [SerializeField] private UserInput _input;
        [SerializeField] private MouseFollowHandle _mouseFollowHandle;
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

        private SoundKeeper _soundKeeper;
        private PlayerBehaviour _playerBehaviour;
        private RaycastToDesk _raycastToDesk;
        private Camera _camera;
        private Backlight _backlight;

        private Action<MoveType> _moveTypeHandler;
        private Action<Coords> _movePawnHandler;
        private Action<WallCoords> _placeWallHandler;
        private Action<GameType> _getGameType;
        private Action<bool> _shouldRestart;
        private Action<PlayerNumber> _lastFollowHanlder;

        private WallCoords _lastWallCoords;

        private void Awake()
        {
            _camera = GetComponent<Camera>();
            _playerBehaviour = GetComponent<PlayerBehaviour>();
            _soundKeeper = GetComponent<SoundKeeper>();
            CoordsConverter = new CoordsConverter(_boardObject.transform.position);
            _raycastToDesk = new RaycastToDesk(_camera, _layerMask, 100f, CoordsConverter);
            _backlight = new Backlight(CoordsConverter, _backlightCellPrefab, _backlightsParent);
        }

        private void Start() => _proxy.StartGame(this, this);
        public void GetMoveType(PlayerNumber playerNumber, Action<MoveType> handler)
        {
            //_moveTypeChoiceMenu.SetActive(true);
            _moveTypeHandler = handler;
            _input.UserType = playerNumber;
            _input.OnLeftMouseButtonClicked2 += MoveTypeClickHandler;
            _input.OnMouseHovering += MoveTypeChoiceHandler;
            _input.IsMouseHoverActive = true;

        }

        public void GetMovePawnCoords(
            PlayerNumber playerNumber,
            IEnumerable<Coords> possibleMoves,
            Action<Coords> handler
        )
        {
            _movePawnHandler = handler;
            _backlight.TurnOnLightOnCells(possibleMoves);
            _input.OnLeftMouseButtonClicked += PawnCoordsClickHandler;
        }

        public void GetPlaceWallCoords(
            PlayerNumber playerNumber,
            IEnumerable<WallCoords> possibleMoves,
            Action<WallCoords> handler
        )
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
                // todo
                //_soundKeeper._pawnMoveSound.PlayNext();
            }
        }

        private void SendPlaceWallCoords(WallCoords coords)
        {
            if (_placeWallHandler != null)
            {
                _placeWallHandler(coords);
                _placeWallHandler = null;
                // todo
                //_soundKeeper._wallMoveSound.PlayNext();
            }
        }

        private void SendMoveType(MoveType moveType)
        {
            if (_moveTypeHandler != null)
            {
                _moveTypeHandler(moveType);
                _moveTypeHandler = null;
            }
        }

        private void MoveTypeClickHandler(PlayerNumber playerNumber)
        {
            if (_raycastToDesk.TryGetCollider(out Collider collider))
            {
                MoveType moveType;
                if (collider.gameObject == _playerBehaviour.GetPawn(playerNumber))
                {
                    moveType = MoveType.MovePawn;
                    _mouseFollowHandle.OnMouseFollowing += PawnFollowHandler;
                    _lastFollowHanlder = PawnFollowHandler;
                }
                else if (collider.gameObject.layer == PlayerBehaviour.GetWallLayer(playerNumber))
                {
                    moveType = MoveType.PlaceWall;
                    _mouseFollowHandle.OnMouseFollowing += WallFollowHandler;
                    _lastFollowHanlder = WallFollowHandler;
                }
                else
                {
                    return;
                }
                _mouseFollowHandle.CurrentPlayer = playerNumber;
                _input.OnLeftMouseButtonClicked2 -= MoveTypeClickHandler;
                _input.OnMouseHovering -= MoveTypeChoiceHandler;
                _input.IsMouseHoverActive = false;
                _playerBehaviour.TurnOffAllHighlight(playerNumber);
                SendMoveType(moveType);
            }
        }

        private void MoveTypeChoiceHandler(PlayerNumber playerNumber)
        {
            if (_raycastToDesk.TryGetCollider(out Collider collider))
            {
                if (collider.gameObject == _playerBehaviour.GetPawn(playerNumber))
                {
                    _playerBehaviour.TryChangePawnHighlight(playerNumber, true);
                }
                else if (collider.gameObject.layer == PlayerBehaviour.GetWallLayer(playerNumber))
                {
                    _playerBehaviour.TryChangeWallsHighlight(playerNumber, true);
                }
                else
                {
                    _playerBehaviour.TurnOffAllHighlight(playerNumber);
                }
            }
            else
            {
                _playerBehaviour.TurnOffAllHighlight(playerNumber);
            }

        }

        private void PawnCoordsClickHandler()
        {
            if (_raycastToDesk.TryGetPawnMoveCoords(out Coords coords))
            {
                _input.OnLeftMouseButtonClicked -= PawnCoordsClickHandler;
                _mouseFollowHandle.OnMouseFollowing -= PawnFollowHandler;   // wrong unfollowing place
                SendMovePawnCoords(coords);
            }
        }

        private void WallCoordsClickHandler()
        {
            if (_raycastToDesk.TryGetPlaceWallCoords(out WallCoords coords))
            {
                _input.OnLeftMouseButtonClicked -= WallCoordsClickHandler;
                _lastFollowHanlder = WallFollowHandler;
                _mouseFollowHandle.OnMouseFollowing -= WallFollowHandler;   // wrong unfollowing place
                SendPlaceWallCoords(coords);
            }
        }

        // need refactor and update
        private void PawnFollowHandler(PlayerNumber playerNumber)
        {
            if (_raycastToDesk.TryRaycast(out RaycastHit hit))
            {
                Transform pawn = _playerBehaviour.GetPawn(playerNumber).transform;
                if (_raycastToDesk.TryGetPawnMoveCoords(out Coords coords))
                {
                    // need sound fix
                    if (CoordsConverter.ToCoords(pawn.position) != coords)
                    {
                        // todo
                        //_soundKeeper._magnetSound.PlayNext();
                        pawn.position = CoordsConverter.ToVector3(coords);
                    }
                }
                else
                {
                    pawn.position = new Vector3(hit.point.x, 1.2f, hit.point.z);
                }
            }
        }

        private void WallFollowHandler(PlayerNumber playerNumber)
        {
            if (!_raycastToDesk.TryRaycast(out RaycastHit hit))
            {
                return;
            }

            Transform wall = _playerBehaviour.GetWall(playerNumber).transform;
            
            if (_raycastToDesk.TryGetPlaceWallCoords(out WallCoords wallCoords, out hit))
            {
                Debug.Log(CoordsConverter.ToWallCoords(hit.point));
                
                // need sound fix
                if (wallCoords != _lastWallCoords)
                {
                    // todo
                    //_soundKeeper._magnetSound.PlayNext();
                    _lastWallCoords = wallCoords;
                }
                
                wall.position = CoordsConverter.ToVector3(wallCoords);
                wall.rotation = CoordsConverter.GetWallQuaternion(wallCoords.Rotation);
            }
            else
            {
                wall.position = new Vector3(hit.point.x, 1.2f, hit.point.z);
            }
        }

        public void MovePawn(
            PlayerInfoContainer<PlayerInfo> playerInfos,
            IEnumerable<WallCoords> wallCoords,
            PlayerNumber playerNumber,
            Coords newCoords
        )
            => _playerBehaviour.MovePawn(playerNumber, newCoords, _proxy.HandleAnimationEnded);

        public void PlaceWall(
            PlayerInfoContainer<PlayerInfo> playerInfos,
            IEnumerable<WallCoords> wallCoords,
            PlayerNumber playerNumber,
            WallCoords newCoords
        ) => _playerBehaviour.PlaceWall(playerInfos, playerNumber, newCoords);

        public void GetGameType(Action<GameType> handler)
        {
            _getGameType = handler;
            _gameTypeChoiceMenu.SetActive(true);
        }

        public void ShowWrongMove(MoveType moveType)
        {
            const float waitingTime = 2.0f;

            _mouseFollowHandle.OnMouseFollowing += _lastFollowHanlder;
            _wrongMoveInfoMenu.SetActive(true);
            StartCoroutine(Waiting(waitingTime, () => _wrongMoveInfoMenu.SetActive(false)));

        }

        public void ShowWinner(PlayerNumber playerNumber) => _winnerInfoMenu.SetActive(true);

        public void ShouldRestart(Action<bool> handler)
        {
            _shouldRestart = handler;
            _restartBlock.SetActive(true);
        }

        public void InitializeField(PlayerInfoContainer<PlayerInfo> playerInfos, IEnumerable<WallCoords> wallCoords)
        {
            _playerBehaviour.ResetWallsPosition(playerInfos);
            MovePawn(playerInfos, wallCoords, PlayerNumber.First, playerInfos.First.PawnCoords);
            MovePawn(playerInfos, wallCoords, PlayerNumber.Second, playerInfos.Second.PawnCoords);
        }

        public void SetGameTypePvP()
        {
            _getGameType(GameType.VersusPlayer);
            _getGameType = null;
        }

        public void SetGameTypePvBot()
        {
            _getGameType(GameType.VersusBot);
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
