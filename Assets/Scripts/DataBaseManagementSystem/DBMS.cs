using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using QuoridorDelta.Controller;
using QuoridorDelta.Controller.Abstractions.DataBase;
using QuoridorDelta.DataBase;
using QuoridorDelta.Model;

namespace QuoridorDelta.DataBaseManagementSystem
{
    public sealed class DBMS : IDataBase
    {
        [NotNull] private readonly DB _db;

        public event Action<GameState, IDBChangeInfo> OnChange;

        public PlayerInfos PlayerInfos => _db.PlayerInfos;
        [NotNull] public IEnumerable<WallCoords> Walls => new List<WallCoords>(_db.Walls);

        private GameState GameState => new GameState(this);

        public DBMS(PlayerInfos playerInfos, Action<GameState, IDBChangeInfo> onChange)
        {
            _db = new DB(playerInfos, new List<WallCoords>());
            OnChange = onChange;

            OnChange?.Invoke(GameState, new DBInitializedInfo(playerInfos));
        }

        public DBMS(PlayerInfos playerInfos, params INotifiable[] notifiables)
            : this(playerInfos, notifiables
                                .Select(n => (Action<GameState, IDBChangeInfo>) n.HandleChange)
                                .Aggregate((current, handler) => current + handler)) { }


        public void MovePawn(PlayerNumber playerNumber, Coords newCoords)
        {
            _db.PlayerInfos = CreateNew(
                PlayerInfos,
                playerNumber,
                new PlayerInfo(newCoords,
                               PlayerInfos[playerNumber].WallCount
                )
            );

            OnChange?.Invoke(GameState, new DBPawnMovedInfo(playerNumber, newCoords));
        }

        public void PlaceWall(PlayerNumber playerNumber, WallCoords newCoords)
        {
            _db.Walls.Add(newCoords);

            _db.PlayerInfos = CreateNew(
                PlayerInfos,
                playerNumber,
                new PlayerInfo(PlayerInfos[playerNumber].PawnCoords,
                               PlayerInfos[playerNumber].WallCount - 1
                )
            );

            OnChange?.Invoke(GameState, new DBWallPlacedInfo(playerNumber, newCoords));
        }

        private static PlayerInfos CreateNew(PlayerInfos old, PlayerNumber changedPlayer, PlayerInfo newPlayer)
            => changedPlayer switch
            {
                PlayerNumber.First => new PlayerInfos(newPlayer, old[PlayerNumber.Second]),
                PlayerNumber.Second => new PlayerInfos(old[PlayerNumber.First], newPlayer),
                _ => throw new ArgumentOutOfRangeException()
            };
    }
}