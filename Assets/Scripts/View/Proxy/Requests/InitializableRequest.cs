using System;
using UnityEngine;

namespace QuoridorDelta.View.Proxy
{
    internal abstract class InitializableRequest<TOutput> : IRequest
    {
        public TOutput Result { get; private set; }
        private bool StartedInitializing { get; set; }
        public bool Initialized { get; private set; }

        private void Initialize(TOutput result)
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