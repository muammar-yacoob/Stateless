using System;
using System.Threading;
using Stateless.House;
using Stateless.House.Events;
using UnityEngine;

namespace Stateless.Player
{
    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerHouseController : MonoBehaviour
    {
        private IHouse currentHouse;
        private CancellationTokenSource cts;
        private bool isUpdating;
        private int playerIndex;

        private void Awake()
        {
            GetComponent<Rigidbody>().isKinematic = true;
            cts = new CancellationTokenSource();
            playerIndex = GetComponent<PlayerMovement>().PlayerIndex;
        }

        private void OnDestroy() => cts?.Cancel();

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<IHouse>(out var newHouse) && newHouse != currentHouse)
            {
                HouseEvents.Instance.EnterHouse();
                cts?.Cancel();
                cts = new CancellationTokenSource();
                currentHouse = newHouse;
                _ = currentHouse.EnterHouseAsync(cts.Token, playerIndex);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent<IHouse>(out var exitingHouse) && exitingHouse == currentHouse)
            {
                HouseEvents.Instance.ExitHouse();
                cts?.Cancel();
                _ = currentHouse.ExitHouseAsync(cts.Token);
                currentHouse = null;
                cts = new CancellationTokenSource();
            }
        }

        private async void Update()
        {
            if (currentHouse == null || isUpdating) return;
            isUpdating = true;

            try
            {
                await currentHouse.UpdateHouseAsync(cts.Token);
            }
            catch (OperationCanceledException)
            {
            }
            finally
            {
                isUpdating = false;
            }
        }
    }
}