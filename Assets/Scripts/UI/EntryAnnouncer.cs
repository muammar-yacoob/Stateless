using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Stateless
{
    public class EntryAnnouncer : MonoBehaviour, IEntryAnnouncer
    {
        [SerializeField] private TMP_Text entryAnnouncementText;
        private RectTransform box;
        private PlayerInputManager inputManager;

        private void Awake()
        {
            entryAnnouncementText.enabled = false;
            inputManager = FindObjectOfType<PlayerInputManager>();
            inputManager.onPlayerJoined += AnnounceEntry;
            box = entryAnnouncementText.GetComponentInParent<RectTransform>();
        }

        private void OnDestroy()
        {
            inputManager.onPlayerJoined -= AnnounceEntry;
        }

        public async void AnnounceEntry(PlayerInput playerInput)
        {
            int playerIndex = playerInput.playerIndex;
            Debug.Log($"Player {playerIndex +1 } joined!");
            entryAnnouncementText.enabled = true;
            entryAnnouncementText.text = $"Player {playerIndex + 1} joined!";
            
            
            box.gameObject.SetActive(true);
            box.localScale = Vector3.zero;
            entryAnnouncementText.DOFade(1, 0);
            await entryAnnouncementText.GetComponentInParent<RectTransform>().DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack).AsyncWaitForCompletion();
            await entryAnnouncementText.DOFade(0, 1f).AsyncWaitForCompletion();
            box.gameObject.SetActive(false);
        }
    }
}
