using System;

namespace QuoridorDelta.View.Proxy
{
    internal sealed class Request<TInput, TOutput> : IInitializableRequest<TOutput>
    {
        public readonly TInput Input;

        public TOutput Result { get; private set; } = default;
        public bool StartedInitializing { get; private set; } = false;
        public bool Initialized { get; private set; } = false;

        public Request(TInput input) => Input = input;

        private void Initialize(TOutput result)
        {
            if (!Initialized)
            {
                Result = result;
                Initialized = true;
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        public Action<TOutput> StartInitializing()
        {
            if (!StartedInitializing)
            {
                StartedInitializing = true;
                return Initialize;
            }
            else
            {
                throw new InvalidOperationException();
            }
        }
    }
}