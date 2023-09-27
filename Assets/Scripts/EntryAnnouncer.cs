using DG.Tweening;
using Player;
using UnityEngine;
using TMPro;

public class EntryAnnouncer : MonoBehaviour, IEntryAnnouncer
{
    [SerializeField] private TMP_Text entryAnnouncementText;

    private void Awake()
    {
        entryAnnouncementText.enabled = false;
        PlayersJoining.OnPlayerJoined += AnnounceEntry;
    }

    public async void AnnounceEntry(int playerIndex)
    {
        entryAnnouncementText.enabled = true;
        entryAnnouncementText.text = $"Player {playerIndex} joined!";
        entryAnnouncementText.DOFade(1, 0.5f).OnComplete(() => entryAnnouncementText.DOFade(0, 1f)).OnComplete(()=> entryAnnouncementText.enabled = false);
    }
}
