using DG.Tweening;
using SparkCore.Runtime.Core;
using Stateless.Player;
using Stateless.Player.Events;
using Stateless.Zombies.Events;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Stateless.UI
{
    public class UIPlayerHealth : InjectableMonoBehaviour
    {
        private int playerIndex;
        [Header("UI Elements")]
        [SerializeField] private TMP_Text healthText;
        [SerializeField] private Image healthBar;
        [Header("Bar Colors")]
        [SerializeField] private Color healthyColor = Color.green;
        [SerializeField] private Color injuredColor = Color.yellow;
        [SerializeField] private Color criticalColor = Color.red;
    
        private float currentHealth;
        private float maxHealth;
        private PlayerStats playerStats;


        protected override void Awake()
        {
            base.Awake();
            SubscribeEvent<PlayerSpawned>(OnPlayerSpawned);
            SubscribeEvent<PlayerDamaged>(OnPlayerDamaged);
        }

        private void OnDisable()
        {
            UnsubscribeEvent<PlayerSpawned>(OnPlayerSpawned);
            UnsubscribeEvent<PlayerDamaged>(OnPlayerDamaged);
        }

        private void OnValidate()
        {
            playerIndex = transform.GetSiblingIndex();
            //if (playerIndex < 0 || playerIndex > 3) Debug.LogError("Player index must be between 0 and 3",this);
        }

        private void OnPlayerSpawned(PlayerSpawned playerSpawned)
        {
            //Debug.Log($"Initializing player {playerIndex} health");
            int spawnedPlayerIndex = playerSpawned.PlayerStats.PlayerIndex;
            if (spawnedPlayerIndex != playerIndex) return;
            Debug.Log($"Player {playerIndex} health initialized");
            maxHealth = PlayersStatsManager.Instance.PlayerStats[spawnedPlayerIndex].Health;
            currentHealth = maxHealth;
            this.playerStats = playerSpawned.PlayerStats;
        }

        private void OnDestroy() => UnsubscribeEvent<PlayerDamaged>(OnPlayerDamaged);
        
        private void OnPlayerDamaged(PlayerDamaged playerDamaged)
        {
            if (playerDamaged.PlayerIndex != playerIndex) return;
            currentHealth -= playerDamaged.Damage;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
            
            UpdateUI();
        }

        private async void UpdateUI()
        {
            float fillValue = currentHealth / maxHealth;
            fillValue = Mathf.Round(fillValue * 100) / 100;
            fillValue = Mathf.Clamp(fillValue, 0, 1);
            //Debug.Log($"Fill value: {fillValue}");
            
            if(healthText == null)
            {
                Debug.LogError("Health text is null", this);
                return;
            }
            
            healthText.text = fillValue.ToString("P0");
            //Debug.Log($"Ouch! {currentHealth}%");
            await healthBar.DOFillAmount(fillValue, 0.2f).AsyncWaitForCompletion();

            if(currentHealth <= 0)
            {
                PublishEvent(new PlayerDied(playerStats));
                return;
            }
            
            if (currentHealth / maxHealth <= 0.2f)
            {
                healthBar.DOKill();
                healthBar.DOFade(0.5f, 0.5f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
                healthBar.color = criticalColor;
            }
            else if (currentHealth / maxHealth <= 0.5f)
            {
                healthBar.DOKill(); 
                healthBar.color = injuredColor;
                healthBar.DOFade(1f, 0.5f); 
            }
            else if (currentHealth / maxHealth > 0.5f)
            {
                healthBar.DOKill();
                healthBar.color = healthyColor;
                healthBar.DOFade(1f, 0.5f); 
            }
        }
    }
}