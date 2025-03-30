using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Stateless.House.Events;
using UnityEngine;

namespace Stateless.House
{
    [RequireComponent(typeof(Collider))]
    public abstract class BaseHouse : MonoBehaviour, IHouse
    {
        [SerializeField] private string houseId;
        [SerializeField] private Sprite speakerSprite;
        [SerializeField] protected string speakerName;
        [Header("Candy")]
        [SerializeField] protected int initialCandyInventory;
        [SerializeField] protected int candyCountPerCollection;
        [SerializeField] protected List<HouseStep> Steps;

        public string HouseName => houseId;
        private AudioSource audioSource;
        private UniTaskCompletionSource _stepReadySource;
        private int playerIndex;
        private int currentCandyInventory;
        const float typingDurationPerCharacter = 0.02f;


        public event Action<int,int> CandyCollected;
        public event Action<IHouse> HouseEntered;

        private  void Awake()
        {
            GetComponent<Collider>().isTrigger = true;
            audioSource = GetComponent<AudioSource>();
            ResetSteps();
            _stepReadySource = new UniTaskCompletionSource();
            currentCandyInventory = initialCandyInventory;
        }


        public virtual async UniTask EnterHouseAsync(CancellationToken token, int playerIndex)
        {
            Debug.Log($"Welcome player{this.playerIndex} to the {speakerName}!");
            this.playerIndex = playerIndex;
            HouseEntered?.Invoke(this);
            await ExecuteStepsAsync(token);
        }

        public virtual async UniTask UpdateHouseAsync(CancellationToken token)
        {
            await UniTask.DelayFrame(1, cancellationToken: token);
        }

        private async UniTask ExecuteStepsAsync(CancellationToken token)
        {
            var candyGiveAway = Mathf.Min(currentCandyInventory, candyCountPerCollection);
            if(candyGiveAway <= 0)
            {
                HouseEvents.Instance.StartDialogue(speakerSprite, speakerName, "We're out of candy, Sorry!", token);
                return;
            }

            foreach (var step in Steps)
            {
                //TODO: Play VoiceOver and animation here
                step.GameObjectsToActivate.ForEach(obj => obj.SetActive(true));
                step.GameObjectsToActivate.ForEach(obj => obj.GetComponent<Renderer>().material.color = step.TintColor);
                
                if(step.VoiceOver != null)
                {
                    audioSource.PlayOneShot(step.VoiceOver);
                    await UniTask.Delay((int)(step.VoiceOver.length * 1000), cancellationToken: token);
                }
                
                _stepReadySource = new UniTaskCompletionSource();
                if(step.DialogText.Length > 0)
                {
                    HouseEvents.Instance.StartDialogue(speakerSprite, speakerName, step.DialogText, token);
                    await _stepReadySource.Task;
                }
                
                await UniTask.DelayFrame(1, cancellationToken: token);
            }
            
            HouseEvents.Instance.StartDialogue(speakerSprite,  speakerName, $"Here's {candyGiveAway} Candies! \nThanks for visiting the {speakerName}! Good night!", token);
            CandyCollected?.Invoke(playerIndex, candyGiveAway);
            currentCandyInventory -= candyGiveAway;
        }

        public void ProceedToNextStep()
        {
            _stepReadySource?.TrySetResult();
        }
        
        public virtual async UniTask ExitHouseAsync(CancellationToken token)
        {
            ResetSteps();
            HouseEvents.Instance.ExitHouse();
            
            await UniTask.DelayFrame(1, cancellationToken: token);
        }

        private void ResetSteps()
        {
            //Reset all the steps
            foreach (var step in Steps)
            {
                step.GameObjectsToActivate.ForEach(obj => obj.SetActive(false));
                step.GameObjectsToActivate.ForEach(obj => obj.GetComponent<Renderer>().material.color = Color.white);
            }
        }
    }
}