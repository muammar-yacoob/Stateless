using System.Collections.Generic;
using SparkCore.Runtime.Core;
using Stateless.Player.Events;
using UnityEngine;

namespace Stateless.UI
{
    public class UIPlayersStatsController : InjectableMonoBehaviour
    {
        [SerializeField] private List<RectTransform> statsPanels;

        protected override void Awake()
        {
            base.Awake();
            statsPanels.ForEach(panel => panel.gameObject.SetActive(false));
            SubscribeEvent<PlayerSpawned>(OnPlayerSpawned);
        }
        
        private void OnDestroy()
        {
            UnsubscribeEvent<PlayerSpawned>(OnPlayerSpawned);
        }
        
        private void OnPlayerSpawned(PlayerSpawned spawned)
        {
            RectTransform statsPanel = statsPanels[spawned.PlayerStats.PlayerIndex];
            statsPanel.gameObject.SetActive(true);
        }
    }
}