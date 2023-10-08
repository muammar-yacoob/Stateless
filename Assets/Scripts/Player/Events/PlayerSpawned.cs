namespace Stateless.Player.Events
{
    public class PlayerSpawned
    {
        public readonly PlayerStats PlayerStats;

        public PlayerSpawned(PlayerStats playerStats)
        {
            PlayerStats = playerStats;
        }
    }
}