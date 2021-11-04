using System;
using System.Collections.Generic;
using QuoridorDelta.Controller;
using QuoridorDelta.Model;
using UnityEngine;

namespace QuoridorDelta.View
{
    public sealed class UnityInput : MonoBehaviour, ISyncInput
    {
        [SerializeField] private GameTypeWindow _gameTypeWindow = default!;
        [SerializeField] private ShouldRestartWindow _shouldRestartWindow = default!;
        [SerializeField] private Field _field = default!;
        [SerializeField] private Backlight _backLight = default!;

        public void GetGameType(Action<GameType> handler) => _gameTypeWindow.Show(handler);

        public void GetMoveType(PlayerNumber playerNumber, Action<MoveType> handler)
            => _field.PlayerInfoHolders[playerNumber].GetMoveType(handler);

        public void GetMovePawnCoords(
            PlayerNumber playerNumber, IEnumerable<Coords> possibleMoves, Action<Coords> handler
        )
        {
            _backLight.TurnOnLightOnCells(possibleMoves);
            _field.PlayerInfoHolders[playerNumber].GetMovePawnCoords(handler + (_ => _backLight.TurnOffLights()));
        }

        public void GetPlaceWallCoords(
            PlayerNumber playerNumber, IEnumerable<WallCoords> possibleWallPlacements, Action<WallCoords> handler
        ) => _field.PlayerInfoHolders[playerNumber].GetPlaceWallCoords(handler);

        public void ShouldRestart(Action<bool> handler) => _shouldRestartWindow.Show(handler);
    }
}
