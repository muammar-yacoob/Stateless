using DG.Tweening;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class EntryAnnouncer : MonoBehaviour, IEntryAnnouncer
{
    [SerializeField] private TMP_Text entryAnnouncementText;
    private PlayerInputManager inputManager;

    private void Awake()
    {
        entryAnnouncementText.enabled = false;
        inputManager = FindObjectOfType<PlayerInputManager>();
        inputManager.onPlayerJoined += AnnounceEntry;
    }

    private void OnDestroy()
    {
        inputManager.onPlayerJoined -= AnnounceEntry;
    }

    public void AnnounceEntry(PlayerInput playerInput)
    {
        int playerIndex = playerInput.playerIndex;
        Debug.Log($"Player {playerIndex +1 } joined!");
        entryAnnouncementText.enabled = true;
        entryAnnouncementText.text = $"Player {playerIndex + 1} joined!";
        entryAnnouncementText.DOFade(1, 0.5f).OnComplete(() => entryAnnouncementText.DOFade(0, 1f)).OnComplete(()=> entryAnnouncementText.enabled = false);
    }
}
