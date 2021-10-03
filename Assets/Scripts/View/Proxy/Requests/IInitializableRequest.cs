using System;

namespace QuoridorDelta.View.Proxy
{
    internal interface IInitializableRequest<TOutput> : IRequest
    {
        public TOutput Result { get; }
        bool StartedInitializing { get; }
        bool Initialized { get; }
        Action<TOutput> StartInitializing();
    }
}