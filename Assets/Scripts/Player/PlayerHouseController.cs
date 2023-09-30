using System;
using System.Threading;
using SparkCore.Runtime.Injection;
using StageSystem;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerHouseController : InjectableMonoBehaviour
    {
        private IHouse currentHouse;
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
            if (!other.TryGetComponent<IHouse>(out var newHouse)) return;
            if (newHouse == currentHouse) return;

            cts?.Cancel();
            cts = new CancellationTokenSource();
            currentHouse = newHouse;
            _ = currentHouse.EnterHouseAsync(cts.Token);
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.TryGetComponent<IHouse>(out var newHouse)) return;
            if (currentHouse != newHouse) return;

            cts?.Cancel();
            _ = currentHouse.ExitHouseAsync(cts.Token);
            currentHouse = null;
            cts = new CancellationTokenSource();
        }


        async void Update()
        {
            if (currentHouse == null || isUpdating) return;

            isUpdating = true;
            try
            {
                await currentHouse.UpdateHouseAsync(cts.Token);
            }
            catch (OperationCanceledException)
            {
                isUpdating = false;
            }
            finally
            {
                isUpdating = false;
            }
        }
    }
}