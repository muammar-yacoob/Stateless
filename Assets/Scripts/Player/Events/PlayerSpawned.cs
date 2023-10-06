using UnityEngine;

namespace Stateless.Player.Events
{
    public class PlayerSpawned
    {
        public readonly GameObject PlayerInstance;
        public readonly PlayerStats PlayerStats;

        public PlayerSpawned(GameObject playerInstance, PlayerStats playerStats)
        {
            PlayerInstance = playerInstance;
            PlayerStats = playerStats;
        }
    }
}