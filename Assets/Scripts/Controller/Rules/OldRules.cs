﻿using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using QuoridorDelta.DataBaseManagementSystem;
using QuoridorDelta.Model;

namespace QuoridorDelta.Controller
{
    public sealed class OldRules : Rules
    {
        private const int MinCoords = 0;
        private const int MinWallCoords = MinCoords;
        private const int MaxCoords = 8;
        private const int MaxWallCoords = 7;

        private static int GetDistance(Coords c1, Coords c2) => Math.Abs(c1.X - c2.X) + Math.Abs(c1.Y - c2.Y);

        private bool IsThereEnemyNearby(PlayerNumber playerNumber)
            => IsThereEnemyNearby(playerNumber, LastGameState.PlayerInfoContainer);

        private static bool IsThereEnemyNearby(PlayerNumber playerNumber, PlayerInfoContainer<PlayerInfo> playerInfos)
            => GetDistance(playerInfos[playerNumber].PawnCoords, playerInfos[playerNumber.Changed()].PawnCoords) == 1;

        private static bool IsWithinFieldRange(Coords coords)
        {
            (int x, int y) = coords;

            return (x >= MinCoords) && (x <= MaxCoords) && (y >= MinCoords) && (y <= MaxCoords);
        }

        private static bool IsWithinFieldRange(WallCoords coords)
        {
            ((int x, int y), _) = coords;

            return (x >= MinWallCoords) && (x <= MaxWallCoords) && (y >= MinWallCoords) && (y <= MaxWallCoords);
        }

        private static bool CanJump2StepsOverCloseEnemy(Coords pawnCoords, Coords enemyCoords, Coords newCoords)
        {
            int coordsIndex;

            if (pawnCoords[0] == enemyCoords[0])
            {
                coordsIndex = 1;
            }
            else if (pawnCoords[1] == enemyCoords[1])
            {
                coordsIndex = 0;
            }
            else
            {
                return false;
            }

            return Math.Sign(enemyCoords[coordsIndex] - pawnCoords[coordsIndex])
                == Math.Sign(newCoords[coordsIndex] - pawnCoords[coordsIndex]);
        }

        private bool IsThereWallBetweenNeighbors(Coords c1, Coords c2)
            => IsThereWallBetweenNeighbors(c1, c2, LastGameState.Walls);

        private static bool IsThereWallBetweenNeighbors(Coords c1, Coords c2, [NotNull] IEnumerable<WallCoords> walls)
        {
            Coords minCoord;
            (int x, int y) = c2 - c1;
            WallCoords possibleWall1;
            WallRotation wallRotation;

            if (x == 0)
            {
                minCoord = y > 0 ? c1 : c2;

                wallRotation = WallRotation.Horizontal;
                possibleWall1 = new WallCoords(minCoord - (1, 0), wallRotation);
            }
            else if (y == 0)
            {
                minCoord = x > 0 ? c1 : c2;

                wallRotation = WallRotation.Vertical;
                possibleWall1 = new WallCoords(minCoord - (0, 1), wallRotation);
            }
            else
            {
                throw new InvalidOperationException();
            }

            var possibleWall2 = new WallCoords(minCoord, wallRotation);

            return walls.Contains(possibleWall1) || walls.Contains(possibleWall2);
        }

        private bool HasBadNeighbors(WallCoords wallCoords) => HasBadNeighbors(wallCoords, LastGameState.Walls);

        private static bool HasBadNeighbors(WallCoords wallCoords, [NotNull] IEnumerable<WallCoords> walls)
        {
            (Coords coords, WallRotation orientation) = wallCoords;

            Coords deltaWallCoords = orientation switch
            {
                WallRotation.Horizontal => (1, 0),
                WallRotation.Vertical => (0, 1),
                _ => throw new ArgumentOutOfRangeException(),
            };

            return walls.Contains((coords + deltaWallCoords, orientation))
                || walls.Contains((coords - deltaWallCoords, orientation));
        }

        public override bool CanMovePawn(PlayerNumber playerNumber, Coords newCoords)
            => CanMovePawn(playerNumber, newCoords, LastGameState);

        public static bool CanMovePawn(PlayerNumber playerNumber, Coords newCoords, [NotNull] GameState gameState)
        {
            PlayerInfo playerInfo = gameState.PlayerInfoContainer[playerNumber];

            int moveDistance = GetDistance(playerInfo.PawnCoords, newCoords);
            Coords otherPawnCoords = gameState.PlayerInfoContainer[playerNumber.Changed()].PawnCoords;

            if ((newCoords == playerInfo.PawnCoords)
             || (!IsWithinFieldRange(newCoords))
             || (moveDistance > 2)
             || (otherPawnCoords == newCoords)
             || (!IsThereEnemyNearby(playerNumber, gameState.PlayerInfoContainer) && moveDistance > 1)
             || (moveDistance > 1 && !CanJump2StepsOverCloseEnemy(playerInfo.PawnCoords, otherPawnCoords, newCoords)))
            {
                return false;
            }

            return moveDistance switch
            {
                2 => !IsThereWallBetweenNeighbors(playerInfo.PawnCoords, otherPawnCoords, gameState.Walls)
                  && !IsThereWallBetweenNeighbors(otherPawnCoords, newCoords, gameState.Walls),
                1 => !IsThereWallBetweenNeighbors(playerInfo.PawnCoords, newCoords, gameState.Walls),
                _ => throw new InvalidProgramException()
            };
        }

        private bool CanPlaceWallWallCountUnchecked(WallCoords newCoords)
            => CanPlaceWallWallCountUnchecked(newCoords, LastGameState);

        private static bool CanPlaceWallWallCountUnchecked(WallCoords newCoords, [NotNull] GameState gameState)
            => (gameState.Walls.All(wall => wall.Coords != newCoords.Coords))
            && (IsWithinFieldRange(newCoords))
            && (!HasBadNeighbors(newCoords, gameState.Walls))
            && (PathExist(newCoords, gameState));

        public override bool CanPlaceWall(PlayerNumber playerNumber, WallCoords newCoords)
            => CanPlaceWall(playerNumber, newCoords, LastGameState);

        public static bool CanPlaceWall(PlayerNumber playerNumber, WallCoords newCoords, [NotNull] GameState gameState)
            => (gameState.PlayerInfoContainer[playerNumber].WallCount > 0)
            && CanPlaceWallWallCountUnchecked(newCoords, gameState);

        public override IEnumerable<Coords> GetPossiblePawnMoves(PlayerNumber playerNumber)
            => GetPossiblePawnMoves(playerNumber, LastGameState);

        [NotNull]
        public static IEnumerable<Coords> GetPossiblePawnMoves(PlayerNumber playerNumber, [NotNull] GameState gameState)
        {
            Coords pawnCoords = gameState.PlayerInfoContainer[playerNumber].PawnCoords;

            var possibleMoves = new List<Coords>();

            for (int x = pawnCoords.X - 2; x <= pawnCoords.X + 2; x++)
            {
                for (int y = pawnCoords.Y - 2; y <= pawnCoords.Y + 2; y++)
                {
                    Coords newCoords = (x, y);

                    if (GetDistance(pawnCoords, newCoords) <= 2 && CanMovePawn(playerNumber, newCoords, gameState))
                    {
                        possibleMoves.Add(newCoords);
                    }
                }
            }

            return possibleMoves;
        }

        public override bool IsWinner(PlayerNumber playerNumber) => IsWinner(playerNumber, LastGameState.PlayerInfoContainer);

        public static bool IsWinner(PlayerNumber playerNumber, PlayerInfoContainer<PlayerInfo> playerInfos)
        {
            int coordsY = playerInfos[playerNumber].PawnCoords.Y;

            return playerNumber switch
            {
                PlayerNumber.First => coordsY == MaxCoords,
                PlayerNumber.Second => coordsY == MinCoords,
                _ => throw new ArgumentOutOfRangeException(),
            };
        }

        private static int GetWinCoordsY(PlayerNumber playerNumber) => playerNumber switch
        {
            PlayerNumber.First => MaxCoords,
            PlayerNumber.Second => MinCoords,
            _ => throw new ArgumentOutOfRangeException()
        };

        public bool PathExist(WallCoords newCoords) => PathExist(newCoords, LastGameState);

        private static bool PathExist(WallCoords newCoords, [NotNull] GameState gameState)
        {
            var wallList = new List<WallCoords>(gameState.Walls) { newCoords };

            return PathExist(GetWinCoordsY(PlayerNumber.First),
                             wallList,
                             gameState.PlayerInfoContainer[PlayerNumber.First].PawnCoords,
                             new HashSet<Coords>())
                && PathExist(GetWinCoordsY(PlayerNumber.Second),
                             wallList,
                             gameState.PlayerInfoContainer[PlayerNumber.Second].PawnCoords,
                             new HashSet<Coords>());
        }

        private static bool PathExist(
            int winCoordsY,
            IEnumerable<WallCoords> walls,
            Coords coords,
            ISet<Coords> visitedCoords
        )
        {
            if (coords.Y == winCoordsY)
            {
                return true;
            }

            visitedCoords.Add(coords);

            foreach (Coords neighbor in GetNeighbors(coords))
            {
                if (visitedCoords.Contains(neighbor) || IsThereWallBetweenNeighbors(coords, neighbor, walls))
                {
                    continue;
                }

                if (PathExist(winCoordsY, walls, neighbor, visitedCoords))
                {
                    return true;
                }
            }

            return false;
        }

        [NotNull]
        private static List<Coords> GetNeighbors(Coords coords)
        {
            var neighbours = new List<Coords>();

            AddIfWithinFieldRange((coords.X - 1, coords.Y), neighbours);
            AddIfWithinFieldRange((coords.X + 1, coords.Y), neighbours);
            AddIfWithinFieldRange((coords.X, coords.Y - 1), neighbours);
            AddIfWithinFieldRange((coords.X, coords.Y + 1), neighbours);

            return neighbours;
        }

        private static void AddIfWithinFieldRange(Coords coords, [NotNull] ICollection<Coords> list)
        {
            if (IsWithinFieldRange(coords))
            {
                list.Add(coords);
            }
        }

        public override IEnumerable<WallCoords> GetPossibleWallPlacements(PlayerNumber playerNumber)
            => GetPossibleWallPlacements(playerNumber, LastGameState);

        [NotNull]
        public static IEnumerable<WallCoords> GetPossibleWallPlacements(PlayerNumber playerNumber, GameState gameState)
        {
            List<WallCoords> possibleCoords = new List<WallCoords>();

            for (int y = 0; y < MaxWallCoords; y++)
            {
                for (int x = 0; x < MaxWallCoords; x++)
                {
                    for (var o = WallRotation.Horizontal; o != WallRotation.Vertical; o = WallRotation.Vertical)
                    {
                        WallCoords wallCoords = ((x, y), o);

                        if (CanPlaceWallWallCountUnchecked(wallCoords, gameState))
                        {
                            possibleCoords.Add(wallCoords);
                        }
                    }
                }
            }

            return possibleCoords;
        }
    }
}
