using System.Collections.Concurrent;
using System.Threading;

namespace QuoridorDelta.View.Proxy
{
    public abstract class GameProxy
    {
        private Thread? _thread;

        internal ConcurrentQueue<IRequest> Requests { get; } = new ConcurrentQueue<IRequest>();

        private protected void Start(ThreadStart starter)
        {
            _thread = new Thread(starter) { Name = "GameThread" };
            _thread.Start();
        }

        private protected TOut WaitRequest<TOut>(InitializableRequest<TOut> request)
        {
            Requests.Enqueue(request);

            while (!request.Initialized)
            {
                const int sleepTime = 100;

                Thread.Sleep(sleepTime);
            }

            return request.Result;
        }

        private protected TOut Wait<TIn, TOut>(TIn input) => WaitRequest(new Request<TIn, TOut>(input));
        private protected TOut Wait<TOut>() => WaitRequest(new InitializableRequest<TOut>());

        private protected void Send<TIn>(TIn input) => Requests.Enqueue(new ActionRequest<TIn>(input));

        public void Dispose() => _thread!.Abort();
    }
}
