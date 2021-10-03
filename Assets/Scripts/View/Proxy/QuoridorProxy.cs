using QuoridorDelta.Controller;
using QuoridorDelta.Model;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuoridorDelta.View.Proxy
{
    // todo
    public sealed class QuoridorProxy : IView, IDisposable
    {
        private readonly Task _task;

        internal ConcurrentQueue<IRequest> Requests { get; } = new ConcurrentQueue<IRequest>();

        public QuoridorProxy() => _task = Task.Run(Start);

        private void Start()
        {
            var gameData = new GameData();
            var gameController = new GameController(gameData, this);
            gameController.Run();
        }

        private TOut WaitRequest<TOut>(IInitializableRequest<TOut> request)
        {
            Requests.Enqueue(request);

            while (!request.Initialized)
            {
                // todo: 100 is fast, 200 is somewhere ok, 500 is slow
                const int sleepTime = 100;

                Task.Delay(sleepTime);
            }

            return request.Result;
        }
        private TOut Wait<TIn, TOut>(TIn input) => WaitRequest(new Request<TIn, TOut>(input));
        private TOut Wait<TOut>() => WaitRequest(new InputlessRequest<TOut>());
        private void Send<TIn>(TIn input) => Requests.Enqueue(new ActionRequest<TIn>(input));

        public void Dispose() => _task.Dispose();


        public GameType GetGameType() => Wait<GameType>();
        public MoveType GetMoveType(PlayerType playerType) => Wait<PlayerType, MoveType>(playerType);
        public Coords GetMovePawnCoords(PlayerType playerType, IEnumerable<Coords> possibleMoves) => Wait<(PlayerType, IEnumerable<Coords>), Coords>((playerType, possibleMoves));
        public WallCoords GetPlaceWallCoords(PlayerType playerType) => Wait<PlayerType, WallCoords>(playerType);
        public void MovePlayerPawn(PlayerType playerType, Coords newCoords) => Send((playerType, newCoords));
        public void PlacePlayerWall(PlayerType playerType, WallCoords newCoords) => Send((playerType, newCoords));
        public void ShowWrongMove(PlayerType playerType, MoveType moveType) => Send((playerType, moveType));
        public void ShowWinner(PlayerType playerType) => Send(playerType);
        public bool ShouldRestart() => Wait<bool>();
    }
}