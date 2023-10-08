using DG.Tweening;
using SparkCore.Runtime.Core;
using Stateless.Candy.Events;
using UnityEngine;

namespace Stateless.Candy
{
    [RequireComponent(typeof(RectTransform))]
    public class UICandyToStats : InjectableMonoBehaviour
    {
        [SerializeField] private int playerIndex;
        [SerializeField] private ParticleSystem arrivalParticle;
        [SerializeField] private RectTransform targetUI;
        private Camera mainCam;

        protected override void Awake()
        {
            base.Awake();
            SubscribeEvent<CandyCollected>(OnCandyToStats);
            mainCam = Camera.main;
        }
        private void OnDestroy() => UnsubscribeEvent<CandyCollected>(OnCandyToStats);

        private void OnValidate() => playerIndex = transform.GetSiblingIndex();

        private async void OnCandyToStats(CandyCollected candyCollected)
        {
            if(targetUI == null)
            {
                Debug.LogError("Target UI is null", this);
                return;
            }
            
            if(candyCollected.PlayerIndex != playerIndex) return;
            var candyTransform = candyCollected.candyTransform;

            var uiDestination = targetUI.position;
            //uiDestination.z = mainCam.nearClipPlane;
            RectTransformUtility.ScreenPointToWorldPointInRectangle(targetUI, uiDestination, mainCam, out var destinationUIElement);
            
            // Scale and move
            await candyTransform.DOScale(1f, 0.5f).SetEase(Ease.OutBounce).AsyncWaitForCompletion();
            await candyTransform.DOMove(destinationUIElement, 1f).SetEase(Ease.InOutQuad).AsyncWaitForCompletion();

            // Final scale adjustments
            await candyTransform.DOScale(2f, 0.2f).AsyncWaitForCompletion();
            await candyTransform.DOScale(0f, 0.2f).AsyncWaitForCompletion();
            
            PlayParticle(candyCollected.CandyValue);
        }

        private void PlayParticle(int candyValue)
        {
            if(arrivalParticle == null) return;
            arrivalParticle.transform.localScale = Vector3.one * candyValue;
            arrivalParticle.Play();
        }
    }
}