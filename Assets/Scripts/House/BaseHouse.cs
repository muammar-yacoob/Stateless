using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Stateless.House.Events;
using UnityEngine;
using System.Linq;

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

            // First step executes without waiting for prompt initially
            if (Steps.Count > 0) await ExecuteStepAsync(Steps[0], token, waitForPromptBeforeAnimation: false, waitForPromptAfterAnimation: stepsFlowType == StepsFlowType.Prompt);

            // Remaining steps wait for prompt before animation (in prompt mode)
            foreach (var step in Steps.Skip(1)) await ExecuteStepAsync(step, token, waitForPromptBeforeAnimation: stepsFlowType == StepsFlowType.Prompt, waitForPromptAfterAnimation: false);

            HouseEvents.Instance.StartDialogue(speakerSprite, speakerName, $"Here's {candyGiveAway} Candies! \nThanks for visiting the {speakerName}! Good night!", token);
            CandyCollected?.Invoke(playerIndex, candyGiveAway);
            currentCandyInventory -= candyGiveAway;
        }

        private async UniTask ExecuteStepAsync(HouseStep step, CancellationToken token, bool waitForPromptBeforeAnimation, bool waitForPromptAfterAnimation)
        {
            // Activate objects and setup UI
            step.GameObjectsToActivate.ForEach(obj => obj.SetActive(true));
            step.GameObjectsToActivate.ForEach(obj => obj.GetComponent<Renderer>().material.color = step.TintColor);

            // Play voiceover and display dialogue
            float voiceOverLength = 0;
            if (step.VoiceOver != null)
            {
                audioSource.PlayOneShot(step.VoiceOver);
                voiceOverLength = step.VoiceOver.length;
            }
            
            _stepReadySource = new UniTaskCompletionSource();
            if (step.DialogText.Length > 0) HouseEvents.Instance.StartDialogue(speakerSprite, speakerName, step.DialogText, token);
            
            // Wait for prompt before animation if needed
            if (waitForPromptBeforeAnimation) await _stepReadySource.Task;
            
            // Play animation
            float animationLength = 0;
            if (!string.IsNullOrEmpty(step.AnimationTrigger))
            {
                var animator = GetComponentInChildren<Animator>();
                animator?.SetTrigger(step.AnimationTrigger);
                animationLength = animator?.GetCurrentAnimatorStateInfo(0).length ?? 0;
            }
            
            // Wait for animation to complete if in continuous mode or if it's the first step in prompt mode
            float delayBetweenSteps = Mathf.Max(animationLength, voiceOverLength);
            if (stepsFlowType == StepsFlowType.Continous || (!waitForPromptBeforeAnimation && stepsFlowType == StepsFlowType.Prompt))
                await UniTask.Delay((int)(delayBetweenSteps * 1000), cancellationToken: token);
                
            // Wait for prompt after animation if needed
            if (waitForPromptAfterAnimation) 
            {
                // Reset the completion source if it was already used
                if (waitForPromptBeforeAnimation) _stepReadySource = new UniTaskCompletionSource();
                await _stepReadySource.Task;
            }
        }

        public void ProceedToNextStep() => _stepReadySource?.TrySetResult();

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