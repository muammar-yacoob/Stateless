using System;
using UnityEngine;

namespace Stateless.Player
{
    public class PlayerStats
    {
        private PlayerStats() => PlayerSpawner.PlayerSpawned += index => PlayerIndex = index;
        public int PlayerIndex { get; private set; }
        public GameObject PlayerInstance { get; }
        
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

        public PlayerStats(GameObject playerInstance, int playerIndex, float initialHealth, int initialCandies)
        {
            PlayerIndex = playerIndex;
            Health = initialHealth;
            Candies = initialCandies;
            PlayerInstance = playerInstance;
        }
    }
}