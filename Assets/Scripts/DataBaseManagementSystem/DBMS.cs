#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using QuoridorDelta.Controller;
using QuoridorDelta.Controller.Abstractions.DataBase;
using QuoridorDelta.DataBase;
using QuoridorDelta.Model;

namespace QuoridorDelta.DataBaseManagementSystem
{
    public sealed class Dbms : IDataBase
    {
        private readonly DB _db;

        public event Action<GameState, IDBChangeInfo>? OnChange;

        public PlayerInfoContainer<PlayerInfo> PlayerInfoContainer => _db.PlayerInfoContainer;
        public IEnumerable<WallCoords> Walls => new List<WallCoords>(_db.Walls);

        private GameState GameState => new GameState(this);

        public Dbms(in PlayerInfoContainer<PlayerInfo> playerInfos, Action<GameState, IDBChangeInfo> onChange)
        {
            _db = new DB(playerInfos, new List<WallCoords>());
            OnChange = onChange;

            OnChange?.Invoke(GameState, new DBInitializedInfo(playerInfos));
        }

        public Dbms(in PlayerInfoContainer<PlayerInfo> playerInfos, params INotifiable[] notifiables) : this(playerInfos,
            notifiables.Select(n => (Action<GameState, IDBChangeInfo>)n.HandleChange)
                       .Aggregate((current, handler) => current + handler)) { }


        public void MovePawn(PlayerNumber playerNumber, in Coords newCoords)
        {
            _db.PlayerInfoContainer = Dbms.CreateNew(PlayerInfoContainer,
                                                     playerNumber,
                                                     new PlayerInfo(newCoords, PlayerInfoContainer[playerNumber].WallCount));

            OnChange?.Invoke(GameState, new DBPawnMovedInfo(playerNumber, newCoords));
        }

        public void PlaceWall(PlayerNumber playerNumber, in WallCoords newCoords)
        {
            _db.Walls.Add(newCoords);

            _db.PlayerInfoContainer = Dbms.CreateNew(PlayerInfoContainer,
                                                     playerNumber,
                                                     new PlayerInfo(PlayerInfoContainer[playerNumber].PawnCoords,
                                                                    PlayerInfoContainer[playerNumber].WallCount - 1));

            OnChange?.Invoke(GameState, new DBWallPlacedInfo(playerNumber, newCoords));
        }

        private static PlayerInfoContainer<PlayerInfo> CreateNew(in PlayerInfoContainer<PlayerInfo> old, PlayerNumber changedPlayer, in PlayerInfo newPlayer)
            => changedPlayer switch
            {
                PlayerNumber.First => new PlayerInfoContainer<PlayerInfo>(newPlayer, old[PlayerNumber.Second]),
                PlayerNumber.Second => new PlayerInfoContainer<PlayerInfo>(old[PlayerNumber.First], newPlayer),
                _ => throw new ArgumentOutOfRangeException()
            };
    }
}
