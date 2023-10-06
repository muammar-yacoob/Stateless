namespace Stateless.Player.Events
{
    public class PlayerDied
    {
        public readonly PlayerStats PlayerStats;

        public PlayerDied(PlayerStats playerStats)
        {
            PlayerStats = playerStats;
        }
    }
}