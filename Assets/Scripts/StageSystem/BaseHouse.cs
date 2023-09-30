using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace StageSystem
{
    [RequireComponent(typeof(Collider))]
    public abstract class BaseHouse : MonoBehaviour, IHouse
    {
        [SerializeField] private string houseId;
        [SerializeField] protected string houseName;
        [Header("Candy")]
        [SerializeField] protected int candyCount;
        [SerializeField] protected int candyPerCollection;
        [SerializeField] protected List<StageStep> Steps;

        public string HouseId => houseId;
        private AudioSource audioSource;

        public event Action<int> CandyCollected;
        public event Action<IHouse> HouseEntered;
        public event Action<IHouse> HouseExited;

        private  void Awake()
        {
            GetComponent<Collider>().isTrigger = true;
            audioSource = GetComponent<AudioSource>();
            ResetSteps();
        }


        public virtual async UniTask EnterHouseAsync(CancellationToken token)
        {
            Debug.Log($"Welcome to the {houseName}!");
            HouseEntered?.Invoke(this);
            await ExecuteStepsAsync(token);
            CandyCollected?.Invoke(candyCount);
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
                
                await UniTask.DelayFrame(1, cancellationToken: token);
            }
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