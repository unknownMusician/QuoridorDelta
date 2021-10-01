using Quoridor.Model;
using System;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace QuoridorDelta.Quoridor
{
    public class UnityProxy : MonoBehaviour
    {
        // todo
        //[SerializeField] private FakeView _fakeView;

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
        // todo
        //private void Start() => StartGame(_fakeView);

        public void StartGame(ISyncView view)
        {
            _view = view;
            _proxy = new QuoridorProxy();
            StartCoroutine(Listening());
        }

        private IEnumerator Listening()
        {
            while (_isAlive)
            {
                // todo
                if (_proxy.MoveTypeRequest != null && !_proxy.MoveTypeRequest.Initialized)
                {
                    _view.GetMoveType(_proxy.MoveTypeRequest.Input, _proxy.MoveTypeRequest.Initialize);
                    Debug.Log($"Initialized MoveTypeRequest");
                }
                else if (_proxy.MovePawnRequest != null && !_proxy.MovePawnRequest.Initialized)
                {
                    _view.GetMovePawnCoords(_proxy.MovePawnRequest.Input, _proxy.MovePawnRequest.Initialize);
                    Debug.Log($"Initialized MovePawnRequest");
                }
                else if (_proxy.PlaceWallRequest != null && !_proxy.PlaceWallRequest.Initialized)
                {
                    _view.GetPlaceWallCoords(_proxy.PlaceWallRequest.Input, _proxy.PlaceWallRequest.Initialize);
                    Debug.Log($"Initialized PlaceWallRequest");
                }

                yield return null;
            }
        }
    }

    public class QuoridorProxy : IView, IDisposable
    {
        private readonly Task _task;

        public Request<PlayerType, MoveType> MoveTypeRequest { get; private set; }
        public Request<PlayerType, Coords> MovePawnRequest { get; private set; }
        public Request<PlayerType, Wall> PlaceWallRequest { get; private set; }

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

        public Wall GetPlaceWallCoords(PlayerType playerType) => WaitFor<Wall>(playerType, value => PlaceWallRequest = value);

        public void Dispose() => _task.Dispose();
    }

    public interface IView
    {
        MoveType GetMoveType(PlayerType playerType);
        Coords GetMovePawnCoords(PlayerType playerType);
        Wall GetPlaceWallCoords(PlayerType playerType);
    }

    public interface ISyncView
    {
        void GetMoveType(PlayerType playerType, Action<MoveType> handler);
        void GetMovePawnCoords(PlayerType playerType, Action<Coords> handler);
        void GetPlaceWallCoords(PlayerType playerType, Action<Wall> handler);
    }

    public sealed class Request<TInput, TOutput>
    {
        public readonly TInput Input;
        public TOutput Result { get; private set; } = default;
        public bool Initialized { get; private set; } = false;

        public Request(TInput input) => Input = input;

        public void Initialize(TOutput result)
        {
            if (!Initialized)
            {
                lock (this)
                {
                    Result = result;
                    Initialized = true;
                }
            }
            else
            {
                throw new InvalidOperationException();
            }
        }
    }
}