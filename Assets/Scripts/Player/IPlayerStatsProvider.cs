using System.Collections.Generic;

namespace Stateless.Player
{
    public interface IPlayerStatsProvider
    {
        IReadOnlyList<PlayerStats> PlayerStats { get; }
        void AddPlayer(PlayerStats playerStat);
        void RemovePlayer(PlayerStats playerStat);
        List<PlayerStats> GetPlayers();
    }
}