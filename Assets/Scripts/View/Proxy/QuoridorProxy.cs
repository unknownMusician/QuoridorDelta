using QuoridorDelta.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuoridorDelta.View.Proxy
{
    public sealed class QuoridorProxy : IView, IDisposable
    {
        private readonly Task _task;

        internal Request<PlayerType, MoveType> MoveTypeRequest { get; private set; }
        internal Request<(PlayerType, IEnumerable<Coords>), Coords> MovePawnRequest { get; private set; }
        internal Request<PlayerType, WallCoords> PlaceWallRequest { get; private set; }

        public QuoridorProxy() => _task = Task.Run(Start);

        private void Start()
        {
            // todo: create Quoridor Game
            new FakeGame(this);
        }

        private TOut WaitFor<TIn, TOut>(TIn input, Action<Request<TIn, TOut>> requestSetter)
        {
            var request = new Request<TIn, TOut>(input);
            requestSetter(request);

            while (!request.Initialized)
            {
                // todo: 100 is fast, 200 is ok, 500 is slow
                const int sleepTime = 200;

                Task.Delay(sleepTime);
            }

            requestSetter(null);
            return request.Result;
        }

        public MoveType GetMoveType(PlayerType playerType) => WaitFor<PlayerType, MoveType>(playerType, value => MoveTypeRequest = value);

        public Coords GetMovePawnCoords(PlayerType playerType, IEnumerable<Coords> possibleMoves)
        {
            return WaitFor<(PlayerType, IEnumerable<Coords>), Coords>((playerType, possibleMoves), value => MovePawnRequest = value);
        }

        public WallCoords GetPlaceWallCoords(PlayerType playerType)
        {
            return WaitFor<PlayerType, WallCoords>(playerType, value => PlaceWallRequest = value);
        }

        public void Dispose() => _task.Dispose();
    }
}