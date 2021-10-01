using System;

namespace QuoridorDelta.View.Proxy
{
    internal sealed class Request<TInput, TOutput>
    {
        public readonly TInput Input;
        public TOutput Result { get; private set; } = default;
        public bool Initialized { get; private set; } = false;

        public Request(TInput input) => Input = input;

        public void Initialize(TOutput result)
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
    }
}