using QuoridorDelta.Model;
using UnityEngine;

namespace QuoridorDelta.View
{
    // todo: remove
    public class FakeGame
    {
        public FakeGame(IView view)
        {
            Debug.Log($".01");
            Debug.Log(view.GetMoveType(PlayerType.First));
            Debug.Log($".02");
            Debug.Log(view.GetMovePawnCoords(PlayerType.First));
            Debug.Log($".03");
            Debug.Log(view.GetPlaceWallCoords(PlayerType.First));
            Debug.Log($".04");
        }
    }
}