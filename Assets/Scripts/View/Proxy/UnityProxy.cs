using QuoridorDelta.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QuoridorDelta.View.Proxy
{
    public sealed class UnityProxy : MonoBehaviour
    {
        private bool _isAlive = true;
        private QuoridorProxy _proxy;
        private ISyncView _view;

        private void OnDestroy()
        {
            _isAlive = false;

            if (_proxy != null)
            {
                _proxy.Dispose();
                _proxy = null;
            }
        }

        public void StartGame(ISyncView view)
        {
            _view = view;
            _proxy = new QuoridorProxy();
            StartCoroutine(Listening());
        }

        private void HandleRequest(IRequest request)
        {
            switch (request)
            {
                case InputlessRequest<GameType> gameTypeRequest:
                    _view.GetGameType(gameTypeRequest.StartInitializing());

                    break;
                case Request<PlayerType, MoveType> moveTypeRequest:
                    _view.GetMoveType(moveTypeRequest.Input,
                                      moveTypeRequest.StartInitializing());

                    break;
                case Request<(PlayerType, IEnumerable<Coords>), Coords> movePawnCoordsRequest:
                    _view.GetMovePawnCoords(movePawnCoordsRequest.Input.Item1,
                                            movePawnCoordsRequest.Input.Item2,
                                            movePawnCoordsRequest.StartInitializing());

                    break;
                case Request<(PlayerType, IEnumerable<WallCoords>), WallCoords> wallCoordsRequest:
                    _view.GetPlaceWallCoords(wallCoordsRequest.Input.Item1,
                                             wallCoordsRequest.Input.Item2,
                                             wallCoordsRequest.StartInitializing());

                    break;
                case ActionRequest<(PlayerType, Coords)> movePawnRequest:
                    _view.MovePawn(movePawnRequest.Input.Item1,
                                   movePawnRequest.Input.Item2);

                    break;
                case ActionRequest<(PlayerType, WallCoords)> placeWallRequest:
                    _view.PlaceWall(placeWallRequest.Input.Item1,
                                    placeWallRequest.Input.Item2);

                    break;
                case ActionRequest<(PlayerType, MoveType)> showWrongMoveRequest:
                    _view.ShowWrongMove(showWrongMoveRequest.Input.Item1,
                                        showWrongMoveRequest.Input.Item2);

                    break;
                case ActionRequest<PlayerType> showWinnerRequest:
                    _view.ShowWinner(showWinnerRequest.Input);

                    break;
                case InputlessRequest<bool> shouldRestartRequest:
                    _view.ShouldRestart(shouldRestartRequest.StartInitializing());

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private IEnumerator Listening()
        {
            while (_isAlive)
            {
                while (!_proxy.Requests.IsEmpty)
                {
                    if (_proxy.Requests.TryDequeue(out IRequest request))
                    {
                        HandleRequest(request);
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }
                }

                yield return null;
            }
        }
    }
}