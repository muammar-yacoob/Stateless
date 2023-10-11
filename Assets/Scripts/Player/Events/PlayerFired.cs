namespace Stateless.Player.Events
{
    public class PlayerFired
    {
        public int FireCost;
        public int DamageAmount;

        public PlayerFired(int fireCost, int damageAmount)
        {
            this.FireCost = fireCost;
            this.DamageAmount = damageAmount;
        }
    }
    
}