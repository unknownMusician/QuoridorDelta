using QuoridorDelta.Model;
using System;
using UnityEngine;

namespace QuoridorDelta.View
{
    // todo: remove
    public class FakeView : MonoBehaviour, ISyncView
    {
        public void GetMoveType(PlayerType playerType, Action<MoveType> handler) => handler(MoveType.PlaceWall);
        public void GetMovePawnCoords(PlayerType playerType, Action<Coords> handler) => handler(new Coords(5, 6));
        public void GetPlaceWallCoords(PlayerType playerType, Action<WallCoords> handler) => handler(((9, 15), WallOrientation.Vertical));
    }
}