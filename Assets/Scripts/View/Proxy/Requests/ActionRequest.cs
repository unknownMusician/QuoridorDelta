namespace QuoridorDelta.View.Proxy
{
    internal sealed class ActionRequest<TInput> : IRequest
    {
        public readonly TInput Input;

        public ActionRequest(TInput input) => Input = input;
    }
}