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
            SubscribeEvent<PlayerSpawned>(OnPlayerSpawned);
            statsPanels.ForEach(panel => panel.gameObject.SetActive(false));
        }

        private void OnPlayerSpawned(PlayerSpawned spawned)
        {
            RectTransform statsPanel = statsPanels[spawned.PlayerIndex];
            statsPanel.gameObject.SetActive(true);
        }
    }
}