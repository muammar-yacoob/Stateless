using DG.Tweening;
using SparkCore.Runtime.Core;
using Stateless.Player.Events;
using TMPro;
using UnityEngine;

namespace Stateless.UI
{
    public class GameAnnouncements : InjectableMonoBehaviour
    {
        [SerializeField] private TMP_Text entryAnnouncementText;
        private RectTransform box;

        protected override void Awake()
        {
            entryAnnouncementText.enabled = false;
            box = entryAnnouncementText.GetComponentInParent<RectTransform>();
            
            SubscribeEvent<PlayerSpawned>(OnPlayerSpawned);
            SubscribeEvent<PlayerDied>(OnPlayerDied);
        }
        
        private void OnDestroy()
        {
            UnsubscribeEvent<PlayerSpawned>(OnPlayerSpawned);
            UnsubscribeEvent<PlayerDied>(OnPlayerDied);
        }
        
        private void OnPlayerSpawned(PlayerSpawned playerSpawned)
        {
            int playerIndex = playerSpawned.PlayerStats.PlayerIndex;
            string msg = $"Player {playerIndex + 1} joined!";
            ShowAnnouncement(msg);
        }
        
        private void OnPlayerDied(PlayerDied playerDied)
        {
            int playerIndex = playerDied.PlayerStats.PlayerIndex;
            int candies = playerDied.PlayerStats.Candies;
            string msg = $"Player {playerIndex + 1} died with {candies} Candies!";
            ShowAnnouncement(msg);
        }
        
        private async void ShowAnnouncement(string message)
        {
            entryAnnouncementText.enabled = true;
            entryAnnouncementText.text = message;
            box.gameObject.SetActive(true);
            box.localScale = Vector3.zero;
            entryAnnouncementText.DOFade(1, 0);
            await entryAnnouncementText.GetComponentInParent<RectTransform>().DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack).AsyncWaitForCompletion();
            await entryAnnouncementText.DOFade(0, 1f).AsyncWaitForCompletion();
            box.gameObject.SetActive(false);
        }
    }
}