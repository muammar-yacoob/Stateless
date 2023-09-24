using System;
using System.Threading;
using SparkCore.Runtime.Injection;
using StageSystem;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerStageController : InjectableMonoBehaviour
    {
        private IStage currentStage;
        private CancellationTokenSource cts;
        private bool isUpdating;

        protected override void Awake()
        {
            base.Awake();
            GetComponent<Rigidbody>().isKinematic = true;
            cts = new CancellationTokenSource();
        }

        private void OnDestroy() => cts?.Cancel();

        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent<IStage>(out var newStage)) return;
            if (newStage == currentStage) return;

            cts?.Cancel();
            cts = new CancellationTokenSource();
            currentStage = newStage;
            _ = currentStage.EnterStageAsync(cts.Token);
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.TryGetComponent<IStage>(out var newStage)) return;
            if (currentStage != newStage) return;

            cts?.Cancel();
            _ = currentStage.ExitStageAsync(cts.Token);
            currentStage = null;
            cts = new CancellationTokenSource();
        }


        async void Update()
        {
            if (currentStage == null || isUpdating) return;

            isUpdating = true;
            try
            {
                await currentStage.UpdateStageAsync(cts.Token);
            }
            catch (OperationCanceledException)
            {
                // Reset isUpdating flag here so Update can run again if needed
            }
            finally
            {
                isUpdating = false;
            }
        }
    }
}