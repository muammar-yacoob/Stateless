using System;
using System.Collections.Generic;

namespace Stateless.Player
{
    public class PlayersStatsManager
    {
        private static readonly Lazy<PlayersStatsManager> lazyInstance = new (() => new PlayersStatsManager());

        public static PlayersStatsManager Instance => lazyInstance.Value;

        private readonly List<PlayerStats> playerStats = new();

        public IReadOnlyList<PlayerStats> PlayerStats => playerStats;

        public void AddPlayer(PlayerStats playerStat) => playerStats.Add(playerStat);

        public void RemovePlayer(PlayerStats playerStat) => playerStats.Remove(playerStat);

        public List<PlayerStats> GetPlayers() => playerStats;
    }
}