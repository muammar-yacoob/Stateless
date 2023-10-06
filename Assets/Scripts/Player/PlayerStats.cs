using System;

namespace Stateless.Player
{
    public class PlayerStats
    {
        public int PlayerIndex { get; }
        
        private float health;
        public float Health 
        {
            get => health;
            set
            {
                health = value;
                HealthChanged?.Invoke(health, PlayerIndex);
            }
        }

        private int candies;
        public int Candies 
        {
            get => candies;
            set
            {
                candies = value;
                CandyCountChanged?.Invoke(candies, PlayerIndex);
            }
        }

        public event Action<float, int> HealthChanged;
        public event Action<int, int> CandyCountChanged;

        public PlayerStats(int playerIndex, float initialHealth, int initialCandies)
        {
            PlayerIndex = playerIndex;
            Health = initialHealth;
            Candies = initialCandies;
        }
    }
}