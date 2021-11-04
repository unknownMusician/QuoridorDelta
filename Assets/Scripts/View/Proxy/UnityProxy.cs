using System;
using System.Collections;
using System.Collections.Generic;
using QuoridorDelta.Controller;
using QuoridorDelta.Model;
using UnityEngine;

namespace QuoridorDelta.View.Proxy
{
    public sealed class UnityProxy : MonoBehaviour
    {
        private bool _isAlive = true;
        private QuoridorProxy? _proxy;
        private ISyncView? _view;
        private ISyncInput? _input;

        private void OnDestroy()
        {
            _isAlive = false;

            if (_proxy != null)
            {
                _proxy.Dispose();
                _proxy = null;
            }
        }

        public void StartGame(ISyncView view, ISyncInput input)
        {
            _view = view;
            _input = input;
            _proxy = new QuoridorProxy();
            StartCoroutine(Listening());
        }

        private void HandleRequest(IRequest request)
        {
            switch (request)
            {
                case InitializableRequest<GameType> gameTypeRequest:
                    _input!.GetGameType(gameTypeRequest.StartInitializing());

                    break;
                case Request<PlayerNumber, MoveType> moveTypeRequest:
                    _input!.GetMoveType(moveTypeRequest.Input, moveTypeRequest.StartInitializing());

                    break;
                case Request<(PlayerNumber, IEnumerable<Coords>), Coords> movePawnCoordsRequest:
                    _input!.GetMovePawnCoords(movePawnCoordsRequest.Input.Item1,
                                              movePawnCoordsRequest.Input.Item2,
                                              movePawnCoordsRequest.StartInitializing());

                    break;
                case Request<(PlayerNumber, IEnumerable<WallCoords>), WallCoords> wallCoordsRequest:
                    _input!.GetPlaceWallCoords(wallCoordsRequest.Input.Item1,
                                               wallCoordsRequest.Input.Item2,
                                               wallCoordsRequest.StartInitializing());

                    break;
                case ActionRequest<PlayerNumber> showWinnerRequest:
                    _view!.ShowWinner(showWinnerRequest.Input);

                    break;
                case ActionRequest<MoveType> showWrongMoveRequest:
                    _view!.ShowWrongMove(showWrongMoveRequest.Input);

                    break;
                case InitializableRequest<bool> shouldRestartRequest:
                    _input!.ShouldRestart(shouldRestartRequest.StartInitializing());

                    break;
                case ActionRequest<(PlayerInfoContainer<PlayerInfo>, IEnumerable<WallCoords>)> fieldInitRequest:
                    _view!.InitializeField(fieldInitRequest.Input.Item1, fieldInitRequest.Input.Item2);

                    break;
                case ActionRequest<(PlayerInfoContainer<PlayerInfo>, IEnumerable<WallCoords>, PlayerNumber, Coords)>
                    movePawnRequest:
                    _view!.MovePawn(movePawnRequest.Input.Item1,
                                    movePawnRequest.Input.Item2,
                                    movePawnRequest.Input.Item3,
                                    movePawnRequest.Input.Item4);

                    break;
                case ActionRequest<(PlayerInfoContainer<PlayerInfo>, IEnumerable<WallCoords>, PlayerNumber,
                    WallCoords)> placeWallRequest:
                    _view!.PlaceWall(placeWallRequest.Input.Item1,
                                     placeWallRequest.Input.Item2,
                                     placeWallRequest.Input.Item3,
                                     placeWallRequest.Input.Item4);

                    break;
                default: throw new ArgumentOutOfRangeException();
            }
        }

        private IEnumerator Listening()
        {
            while (_isAlive)
            {
                if (!_proxy!.Requests.IsEmpty)
                {
                    if (_proxy.Requests.TryDequeue(out IRequest request))
                    {
                        HandleRequest(request);
                    }
                    else
                    {
                        throw new InvalidProgramException();
                    }
                }

                yield return null;
            }
        }
    }
}
