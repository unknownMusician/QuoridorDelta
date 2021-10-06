using System;
using JetBrains.Annotations;
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
            if (Initialized)
            {
                throw new InvalidOperationException();
            }

            Result = result;
            Initialized = true;
        }

        [NotNull]
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
