namespace Stateless.Zombies.Events
{
    public class PlayerDamaged
    {
        public readonly float Damage;
        public readonly int PlayerIndex;

        public PlayerDamaged(float damage, int playerIndex)
        {
            Damage = damage;
            PlayerIndex = playerIndex;
        }
    }
}