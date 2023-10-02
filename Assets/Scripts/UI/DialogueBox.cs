using System.Threading;
using DG.Tweening;
using Stateless.House.Events;
using TMPro;
using UnityEngine;

namespace Stateless.UI
{
    public class DialogueBox : MonoBehaviour
    {
        private TMP_Text dialogueText;
        private RectTransform dialogueBox;
        private float initialY;
        private CancellationTokenSource cts;

        private void Awake()
        {
            dialogueBox ??= GetComponent<RectTransform>();
            dialogueText ??= GetComponentInChildren<TMP_Text>();
            initialY = dialogueBox.position.y;
        }

        private void OnEnable()
        {
            HouseEvents.Instance.DialogueStarted += OnDialogueStarted;
            HouseEvents.Instance.HouseEntered += OnHouseEntered;
            HouseEvents.Instance.HouseExited += OnHouseExited;
            
            var position = dialogueBox.position;
            dialogueBox.position = new Vector3(position.x, initialY - dialogueBox.rect.height, position.z);
        }

        private void OnDisable()
        {
            HouseEvents.Instance.DialogueStarted -= OnDialogueStarted;
            HouseEvents.Instance.HouseEntered -= OnHouseEntered;
            HouseEvents.Instance.HouseExited -= OnHouseExited;
        }
        
        private async void OnDialogueStarted(string dialogue, CancellationToken token)
        {
            if (cts != null)
            {
                cts.Cancel();
                cts.Dispose();
                cts = null;
            }
            
            cts = new CancellationTokenSource();
            await dialogueText.DOText(dialogue, 0.02f, true, cts.Token);
        }
        
        private async void OnHouseEntered()
        {
            var position = dialogueBox.position;
            await dialogueBox.DOMove(new Vector3(position.x, initialY , position.z), 0.5f).SetEase(Ease.InBack).AsyncWaitForCompletion();
        }
        
        private async void OnHouseExited()
        {
            var position = dialogueBox.position;
            await dialogueBox.DOMove(new Vector3(position.x, initialY - dialogueBox.rect.height, position.z), 0.5f).SetEase(Ease.OutBack).AsyncWaitForCompletion();
        }
    }
}