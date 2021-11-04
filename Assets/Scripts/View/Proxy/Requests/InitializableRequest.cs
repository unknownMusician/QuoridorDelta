using System;

namespace QuoridorDelta.View.Proxy
{
    internal class InitializableRequest<TOutput> : IRequest
    {
        public TOutput Result { get; private set; } = default!;
        private bool StartedInitializing { get; set; }
        public bool Initialized { get; private set; }

        private void Initialize(TOutput result)
        {
            if (Initialized)
            {
                throw new InvalidOperationException();
            }

            Result = result;
            Initialized = true;
        }

        public Action<TOutput> StartInitializing()
        {
            if (StartedInitializing)
            {
                throw new InvalidOperationException();
            }

            StartedInitializing = true;

            return Initialize;
        }
    }
}
