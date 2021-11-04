using QuoridorDelta.Model;

namespace QuoridorDelta.Controller
{
    public static class PlayerNumberExtension
    {
        public static PlayerNumber Change(this ref PlayerNumber playerNumber) => playerNumber = playerNumber.Changed();
        public static PlayerNumber Changed(this PlayerNumber playerNumber) => 1 - playerNumber;
    }
}
