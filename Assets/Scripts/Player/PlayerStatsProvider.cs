using System.Collections.Generic;
using System.Linq;
using SparkCore.Runtime.Injection;

namespace Stateless.Player
{
    [RuntimeObject(RuntimeObjectType.Singleton)]
    public class PlayerStatsProvider : IPlayerStatsProvider
    {
        private readonly List<PlayerStats> playerStats = new();
        public IReadOnlyList<PlayerStats> PlayerStats => playerStats;
        public void AddPlayer(PlayerStats playerStat) => playerStats.Add(playerStat);
        public void RemovePlayer(PlayerStats playerStat) => playerStats.Remove(playerStat);
        public List<PlayerStats> GetPlayers() => playerStats;
    }
}