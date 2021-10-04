namespace QuoridorDelta.View.Proxy
{
    internal sealed class Request<TInput, TOutput> : InitializableRequest<TOutput>
    {
        public readonly TInput Input;

        public Request(TInput input) => Input = input;
    }
}