using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace Candy
{
    public class CandyManager : MonoBehaviour
    {
        private Dictionary<string, int> playerScores = new();
        public event Action OnGameOver;
        private int targetCandyCount = 10;

        [SerializeField] private TextMeshProUGUI candyCounterUI;

        public void OnPlayerCollectCandy(string playerId, int candy)
        {
            if (!playerScores.ContainsKey(playerId))
            {
                playerScores[playerId] = 0;
            }

            playerScores[playerId] += candy;
            candyCounterUI.text = $"Player {playerId}: {playerScores[playerId]}";

            if(playerScores[playerId] >= targetCandyCount/2)
            {
                Debug.Log($"Player {playerId} has a sweet tooth!");
            }
            
            if (playerScores.Values.Sum() >= targetCandyCount)
            {
                Debug.Log("Game Over!");
                OnGameOver?.Invoke();
            }
        }
    }
}