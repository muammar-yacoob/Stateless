using SparkCore.Runtime.Injection;
using System;
using SparkCore.Runtime.Core;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayersJoining : InjectableMonoBehaviour
    {
        [SerializeField] private int maxPlayers = 4;
        [SerializeField] private PlayerInput playerInput;
        public static event Action<int> OnPlayerJoined;

        private int currentPlayerCount = 0;

        protected override void Awake()
        {
            playerInput = GetComponent<PlayerInput>();
            playerInput.onActionTriggered += OnJoin;
            playerInput.onDeviceLost += OnDeviceLost;
            playerInput.onDeviceRegained += OnDeviceRegained;
        }

        private void OnDestroy()
        {
            playerInput.onActionTriggered -= OnJoin;
            playerInput.onDeviceLost -= OnDeviceLost;
            playerInput.onDeviceRegained -= OnDeviceRegained;
        }

        private void OnJoin(InputAction.CallbackContext ctx)
        {
            if(ctx.action.name != "Join") return;
            if(currentPlayerCount < maxPlayers)
            {
                OnPlayerJoined?.Invoke(++currentPlayerCount);
            }
            else
            {
                //Debug.Log($"Max players of {maxPlayers} reached");
            }
        }
        
        private void OnDeviceRegained(PlayerInput playerInput)
        {
            Debug.Log($"Device Regained for player {playerInput.playerIndex}");
        }

        private void OnDeviceLost(PlayerInput playerInput)
        {
            Debug.Log($"Device Lost for player {playerInput.playerIndex}");
        }
    }
}