using System;
using System.Collections.Generic;
using System.Linq;
using House;
using SparkCore.Runtime.Utils;
using TMPro;
using UnityEngine;

namespace Candy
{
    public class CandyManager : PersistentSingleton<CandyManager>
    {
        private Dictionary<int, int> playerScores = new();
        [SerializeField] private List<TMP_Text> candyCounterUI;
        public event Action OnGameOver;
        private int targetCandyCount = 20;

        private void Start()
        {
            candyCounterUI.ForEach(x => x.text = $"x0");
            var houses = FindObjectsOfType<BaseHouse>();
            houses.ToList().ForEach(h => h.CandyCollected += OnPlayerCollectCandy);
        }

        private void OnDestroy()
        {
            var houses = FindObjectsOfType<BaseHouse>();
            houses.ToList().ForEach(h => h.CandyCollected -= OnPlayerCollectCandy);
        }

        private void OnPlayerCollectCandy(int playerId, int candy)
        {
            playerScores[playerId] += candy;
            candyCounterUI[playerId].text = $"x{playerScores[playerId]}";

            if (playerScores.Values.Sum() >= targetCandyCount)
            {
                Debug.Log("Mission Complete!");
                OnGameOver?.Invoke();
            }
        }
    }
}