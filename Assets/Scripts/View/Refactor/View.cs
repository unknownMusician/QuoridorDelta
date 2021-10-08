using System;
using System.Collections.Generic;
using QuoridorDelta.Controller;
using QuoridorDelta.Model;
using UnityEngine;

namespace QuoridorDelta.View.Refactor
{
    public sealed class View : MonoBehaviour, ISyncView
    {
        [SerializeField] private WinnerWindow _winnerWindow;
        [SerializeField] private WrongMoveWindow _wrongMoveWindow;
        [SerializeField] private Field _field;

        private void OnValidate() => this.ValidateNull(_winnerWindow, _wrongMoveWindow, _field);

        public void ShowWinner(PlayerNumber playerNumber) => _winnerWindow.Show(playerNumber);

        public void ShowWrongMove(MoveType moveType) => _wrongMoveWindow.Show();

        public void InitializeField(PlayerInfoContainer<PlayerInfo> playerInfos, IEnumerable<WallCoords> wallCoords)
        {
            foreach ((PlayerNumber number, PlayerInfoHolder holder) in _field.PlayerInfoHolders.Pairs)
            {
                holder.ResetElements(playerInfos[number].PawnCoords);
            }
        }

        public void MovePawn(
            PlayerInfoContainer<PlayerInfo> playerInfos,
            IEnumerable<WallCoords> wallCoords,
            PlayerNumber playerNumber,
            Coords newCoords
        ) => _field.PlayerInfoHolders[playerNumber].MovePawn(newCoords);

        public void PlaceWall(
            PlayerInfoContainer<PlayerInfo> playerInfos,
            IEnumerable<WallCoords> wallCoords,
            PlayerNumber playerNumber,
            WallCoords newCoords
        ) => _field.PlayerInfoHolders[playerNumber].PlaceWall(newCoords);
    }
}
