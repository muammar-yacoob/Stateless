using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace House
{
    [RequireComponent(typeof(Collider))]
    public abstract class BaseHouse : MonoBehaviour, IHouse
    {
        [SerializeField] private string houseId;
        [SerializeField] protected string houseName;
        [Header("Candy")]
        [SerializeField] protected int candyCount;
        [SerializeField] protected int candyPerCollection;
        [SerializeField] protected List<HouseStep> Steps;

        public string HouseName => houseId;
        private AudioSource audioSource;
        private UniTaskCompletionSource _stepReadySource;
        private int playerIndex;

        public event Action<int,int> CandyCollected;
        public event Action<IHouse> HouseEntered;
        public event Action<IHouse> HouseExited;

        private  void Awake()
        {
            GetComponent<Collider>().isTrigger = true;
            audioSource = GetComponent<AudioSource>();
            ResetSteps();
            _stepReadySource = new UniTaskCompletionSource();
        }


        public virtual async UniTask EnterHouseAsync(CancellationToken token, int playerIndex)
        {
            Debug.Log($"Welcome player{this.playerIndex} to the {houseName}!");
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
                await _stepReadySource.Task;
                
                await UniTask.DelayFrame(1, cancellationToken: token);
            }
            Debug.Log("No more steps! Goodbye! Player" + playerIndex);
            CandyCollected?.Invoke(playerIndex, candyPerCollection);
            //ResetSteps();
            
        }
        
        public void ProceedToNextStep()
        {
            _stepReadySource?.TrySetResult();
        }
        
        public virtual async UniTask ExitHouseAsync(CancellationToken token)
        {
            ResetSteps();

            Debug.Log($"Thanks for visiting the {houseName}! Goodbye!");
            
            HouseExited?.Invoke(this);
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