using QuoridorDelta.Model;
using System;
using System.Threading.Tasks;

namespace QuoridorDelta.View.Proxy
{
    public sealed class QuoridorProxy : IView, IDisposable
    {
        private readonly Task _task;

        internal Request<PlayerType, MoveType> MoveTypeRequest { get; private set; }
        internal Request<PlayerType, Coords> MovePawnRequest { get; private set; }
        internal Request<PlayerType, WallCoords> PlaceWallRequest { get; private set; }

        public QuoridorProxy() => _task = Task.Run(Start);

        private void Start()
        {
            // todo: create Quoridor Game
            new FakeGame(this);
        }

        private T WaitFor<T>(PlayerType playerType, Action<Request<PlayerType, T>> requestSetter)
        {
            var request = new Request<PlayerType, T>(playerType);
            requestSetter(request);

            while (!request.Initialized)
            {
                // todo: 100 is fast, 200 is ok, 500 is slow
                const int sleepTime = 200;

                Task.Delay(sleepTime);
            }
            return request.Result;
        }

        public MoveType GetMoveType(PlayerType playerType) => WaitFor<MoveType>(playerType, value => MoveTypeRequest = value);

        public Coords GetMovePawnCoords(PlayerType playerType) => WaitFor<Coords>(playerType, value => MovePawnRequest = value);

        public WallCoords GetPlaceWallCoords(PlayerType playerType) => WaitFor<WallCoords>(playerType, value => PlaceWallRequest = value);

        public void Dispose() => _task.Dispose();
    }
}