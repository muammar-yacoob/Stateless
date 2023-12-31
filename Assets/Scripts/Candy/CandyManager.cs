﻿using System;
using System.Collections.Generic;
using System.Linq;
using SparkCore.Runtime.Utils;
using Stateless.House;
using TMPro;
using UnityEngine;

namespace Stateless.Candy
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
            Debug.Log($"{houses.Count()} houses found!");
        }

        private void OnDestroy()
        {
            var houses = FindObjectsOfType<BaseHouse>();
            houses.ToList().ForEach(h => h.CandyCollected -= OnPlayerCollectCandy);
        }

        private void OnPlayerCollectCandy(int playerId, int candy)
        {
            playerScores.TryAdd(playerId, 0);
            playerScores[playerId] += candy;
            candyCounterUI[playerId].text = $"x{playerScores[playerId]}";

            if (playerScores.Values.Sum() >= 5)
            {
                
            }
            
            if (playerScores.Values.Sum() >= targetCandyCount)
            {
                Debug.Log("Mission Complete!");
                OnGameOver?.Invoke();
            }
        }
    }
}