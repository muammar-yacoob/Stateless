using System.Threading;
using DG.Tweening;
using Stateless.House.Events;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Stateless.UI
{
    public class DialogueBox : MonoBehaviour
    {
        [SerializeField] private TMP_Text speakerNameText;
        [SerializeField] private Image speakerImage;
        [SerializeField] private TMP_Text dialogueText;
        private RectTransform canvas;
        private float initialY;
        private CancellationTokenSource cts;

        private void Awake()
        {
            canvas ??= GetComponent<RectTransform>();
            initialY = canvas.position.y;
        }

        private void OnEnable()
        {
            HouseEvents.Instance.DialogueStarted += OnDialogueStarted;
            HouseEvents.Instance.HouseEntered += OnHouseEntered;
            HouseEvents.Instance.HouseExited += OnHouseExited;
            
            var position = canvas.position;
            canvas.position = new Vector3(position.x, initialY - canvas.rect.height, position.z);
        }

        private void OnDisable()
        {
            HouseEvents.Instance.DialogueStarted -= OnDialogueStarted;
            HouseEvents.Instance.HouseEntered -= OnHouseEntered;
            HouseEvents.Instance.HouseExited -= OnHouseExited;
        }
        
        private async void OnDialogueStarted(Sprite speakerSprite, string speakerName, string dialogue, CancellationToken token)
        {
            if (cts != null)
            {
                cts.Cancel();
                cts.Dispose();
                cts = null;
            }
            
            cts = new CancellationTokenSource();
            speakerImage.sprite = speakerSprite;
            speakerNameText.text = speakerName;
            await dialogueText.DOText(dialogue, 0.02f, true, cts.Token);
        }
        
        private async void OnHouseEntered()
        {
            var position = canvas.position;
            await canvas.DOMove(new Vector3(position.x, initialY , position.z), 0.5f).SetEase(Ease.InBack).AsyncWaitForCompletion();
        }
        
        private async void OnHouseExited()
        {
            var position = canvas.position;
            await canvas.DOMove(new Vector3(position.x, initialY - canvas.rect.height, position.z), 0.5f).SetEase(Ease.OutBack).AsyncWaitForCompletion();
        }
    }
}