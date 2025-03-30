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
        [SerializeField] protected StepsFlowType stepsFlowType;
        [SerializeField] private Sprite speakerSprite;
        [SerializeField] protected string speakerName;
        [Header("Candy")]
        [SerializeField] protected int initialCandyInventory;
        [SerializeField] protected int candyCountPerCollection;
        [SerializeField] protected List<HouseStep> Steps;

        private AudioSource audioSource;
        private UniTaskCompletionSource _stepReadySource;
        private int playerIndex;
        private int currentCandyInventory;
        const float typingDurationPerCharacter = 0.02f;


        public event Action<int, int> CandyCollected;
        public event Action<IHouse> HouseEntered;

        public enum StepsFlowType
        {
            Continous,
            Prompt
        }

        private void Awake()
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
            if (candyGiveAway <= 0)
            {
                HouseEvents.Instance.StartDialogue(speakerSprite, speakerName, "We're out of candy, Sorry!", token);
                return;
            }

            // Track if this is the first step
            bool isFirstStep = true;

            foreach (var step in Steps)
            {
                //Activate Objects 
                step.GameObjectsToActivate.ForEach(obj => obj.SetActive(true));
                //Tint Objects
                step.GameObjectsToActivate.ForEach(obj => obj.GetComponent<Renderer>().material.color = step.TintColor);

                //Play VoiceOver
                float voiceOverLength = 0;
                if (step.VoiceOver != null)
                {
                    audioSource.PlayOneShot(step.VoiceOver);
                    voiceOverLength = step.VoiceOver.length;
                    // await UniTask.Delay((int)(step.VoiceOver.length * 1000), cancellationToken: token);
                }

                _stepReadySource = new UniTaskCompletionSource();
                if (step.DialogText.Length > 0)
                {
                    HouseEvents.Instance.StartDialogue(speakerSprite, speakerName, step.DialogText, token);
                }

                // For steps after the first one, wait for user to proceed before playing animation
                if (stepsFlowType == StepsFlowType.Prompt && !isFirstStep) 
                {
                    await _stepReadySource.Task;
                }

                //Play Animation
                float animationLength = 0;
                if (!string.IsNullOrEmpty(step.AnimationTrigger))
                {
                    var animator = GetComponentInChildren<Animator>();
                    animator?.SetTrigger(step.AnimationTrigger);
                    animationLength = animator?.GetCurrentAnimatorStateInfo(0).length ?? 0;
                }

                float delayBetweenSteps = Mathf.Max(animationLength, voiceOverLength);

                // For continuous mode, or for the first step in prompt mode, wait for animation/voiceover
                if (stepsFlowType == StepsFlowType.Continous || (stepsFlowType == StepsFlowType.Prompt && isFirstStep))
                {
                    await UniTask.Delay((int)(delayBetweenSteps * 1000), cancellationToken: token);
                    
                    // After the first step in prompt mode, wait for user input before proceeding to the next step
                    if (stepsFlowType == StepsFlowType.Prompt && isFirstStep)
                    {
                        await _stepReadySource.Task;
                    }
                }
                
                // After the first step, we should wait for prompt before each subsequent step
                isFirstStep = false;
            }

            HouseEvents.Instance.StartDialogue(speakerSprite, speakerName, $"Here's {candyGiveAway} Candies! \nThanks for visiting the {speakerName}! Good night!", token);
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

                var anim = GetComponentInChildren<Animator>();
                anim?.SetBool(step.AnimationTrigger, false);
            }
        }
    }
}