using System;
using UnityEngine;

namespace QuoridorDelta.View.Proxy
{
    internal abstract class InitializableRequest<TOutput> : IRequest
    {
        public TOutput Result { get; protected set; } = default;
        public bool StartedInitializing { get; private set; } = false;
        public bool Initialized { get; private set; } = false;
        protected void Initialize(TOutput result)
        {
            if (!Initialized)
            {
                Result = result;
                Initialized = true;
                Debug.Log($"{this.GetType().Name} Initialized");
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
                Debug.Log($"{this.GetType().Name} Started Initializing");
                return Initialize;
            }
            else
            {
                throw new InvalidOperationException();
            }
        }
    }
}