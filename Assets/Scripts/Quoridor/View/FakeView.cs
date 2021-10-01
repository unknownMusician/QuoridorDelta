using Quoridor.Model;
using System;
using UnityEngine;

namespace QuoridorDelta.Quoridor
{
    // todo: remove
    public class FakeView : MonoBehaviour, ISyncView
    {
        public void GetMoveType(PlayerType playerType, Action<MoveType> handler) => handler(MoveType.PlaceWall);
        public void GetMovePawnCoords(PlayerType playerType, Action<Coords> handler) => handler(new Coords(5, 6));
        public void GetPlaceWallCoords(PlayerType playerType, Action<Wall> handler) => handler(new Wall(WallOrientation.Vertical, new Coords(9, 15)));
    }
}