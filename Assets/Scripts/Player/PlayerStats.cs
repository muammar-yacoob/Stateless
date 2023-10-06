using System;

namespace Stateless.Player
{
    public class PlayerStats
    {
        public int PlayerId { get; }
        
        private float health;
        public float Health 
        {
            get => health;
            set
            {
                health = value;
                HealthChanged?.Invoke(health, PlayerId);
            }
        }

        private int candies;
        public int Candies 
        {
            get => candies;
            set
            {
                candies = value;
                CandyCountChanged?.Invoke(candies, PlayerId);
            }
        }

        public event Action<float, int> HealthChanged;
        public event Action<int, int> CandyCountChanged;

        public PlayerStats(int playerId, float initialHealth, int initialCandies)
        {
            PlayerId = playerId;
            Health = initialHealth;
            Candies = initialCandies;
        }
    }
}