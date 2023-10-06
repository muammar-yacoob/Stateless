using Cysharp.Threading.Tasks;
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
        [SerializeField] private int playerIndex;
        [Header("UI Elements")]
        [SerializeField] private TMP_Text healthText;
        [SerializeField] private Image healthBar;
        [Header("Bar Colors")]
        [SerializeField] private Color healthyColor = Color.green;
        [SerializeField] private Color injuredColor = Color.yellow;
        [SerializeField] private Color criticalColor = Color.red;
    
        private float currentHealth;
        private float maxHealth;


        protected override void Awake()
        {
            base.Awake();
            UniTask.Delay(1000);
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
            if (playerIndex < 0 || playerIndex > 3)
                Debug.LogError("Player index must be between 0 and 3",this);
        }

        private void OnPlayerSpawned(PlayerSpawned playerStats)
        {
            if (playerStats.PlayerIndex != playerIndex) return;
            maxHealth = PlayersStatsManager.Instance.PlayerStats[playerStats.PlayerIndex].Health;
            currentHealth = maxHealth;
        }

        private void OnDestroy() => UnsubscribeEvent<PlayerDamaged>(OnPlayerDamaged);
        
        private void OnPlayerDamaged(PlayerDamaged playerDamaged)
        {
            if (playerDamaged.PlayerIndex != playerIndex) return;
            currentHealth -= playerDamaged.Damage;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
            
            UpdateUI();
        }

        private void UpdateUI()
        {
            float fillValue = currentHealth / maxHealth;
            
            if(healthText == null)
            {
                Debug.LogError("Health text is null", this);
                return;
            }
            
            healthText.text = fillValue.ToString("P0");
            healthBar.DOFillAmount(fillValue, 0.2f);
        
            if (currentHealth / maxHealth <= 0.2f)
            {
                healthBar.DOFade(0.5f, 0.5f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
                healthBar.color = Color.red;
            }
            else if (currentHealth / maxHealth <= 0.5f)
            {
                healthBar.DOKill(); 
                healthBar.color = Color.yellow;
                healthBar.DOFade(1f, 0.5f); 
            }
            else
            {
                healthBar.DOKill(); 
                healthBar.color = Color.green;
                healthBar.DOFade(1f, 0.5f); 
            }
            Debug.Log($"Ouch! {currentHealth}%");
        }
    }
}