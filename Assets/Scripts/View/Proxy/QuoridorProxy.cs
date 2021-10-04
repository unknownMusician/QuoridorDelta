using QuoridorDelta.Controller;
using QuoridorDelta.Model;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

namespace QuoridorDelta.View.Proxy
{
    public sealed class QuoridorProxy : IView, IDisposable
    {
        private readonly Thread _thread;

        internal ConcurrentQueue<IRequest> Requests { get; } = new ConcurrentQueue<IRequest>();

        public QuoridorProxy()
        {
            _thread = new Thread(Start) {Name = "QuoridorThread"};
            _thread.Start();
        }

        private void Start() => new GameController(new GameData(), this).Run();

        private TOut WaitRequest<TOut>(InitializableRequest<TOut> request)
        {
            Requests.Enqueue(request);

            while (!request.Initialized)
            {
                const int sleepTime = 100;

                Thread.Sleep(sleepTime);
            }

            return request.Result;
        }

        private TOut Wait<TIn, TOut>(TIn input) => WaitRequest(new Request<TIn, TOut>(input));
        private TOut Wait<TOut>() => WaitRequest(new InputlessRequest<TOut>());
        private void Send<TIn>(TIn input) => Requests.Enqueue(new ActionRequest<TIn>(input));

        public void Dispose() => _thread.Abort();

        public GameType GetGameType() => Wait<GameType>();
        public MoveType GetMoveType(PlayerType playerType) => Wait<PlayerType, MoveType>(playerType);

        public Coords GetMovePawnCoords(PlayerType playerType, IEnumerable<Coords> possibleMoves) =>
            Wait<(PlayerType, IEnumerable<Coords>), Coords>((playerType, possibleMoves));

        public WallCoords GetPlaceWallCoords(PlayerType playerType) => Wait<PlayerType, WallCoords>(playerType);
        public void MovePlayerPawn(PlayerType playerType, Coords newCoords) => Send((playerType, newCoords));
        public void PlacePlayerWall(PlayerType playerType, WallCoords newCoords) => Send((playerType, newCoords));
        public void ShowWrongMove(PlayerType playerType, MoveType moveType) => Send((playerType, moveType));
        public void ShowWinner(PlayerType playerType) => Send(playerType);
        public bool ShouldRestart() => Wait<bool>();
    }
}