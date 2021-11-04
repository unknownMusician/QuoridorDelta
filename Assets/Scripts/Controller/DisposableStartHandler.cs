using System;

namespace QuoridorDelta.Controller
{
    internal sealed class DisposableStartHandler
    {
        private bool _isStarted = false;

        public void HandleStart()
        {
            if (_isStarted)
            {
                throw new InvalidOperationException();
            }

            _isStarted = true;
        }
    }
}
