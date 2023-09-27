using SparkCore.Runtime.Injection;
using VContainer;
using System;
using UnityEngine;

namespace Player
{
    public class PlayersJoining : InjectableMonoBehaviour
    {
        [SerializeField] private int maxPlayers = 4;
        [Inject] public IPlayerInput playerInput;  
        public static event Action<int> OnPlayerJoined;

        private int currentPlayerCount = 0;

        private void Start()
        {
            playerInput.Join += OnJoin;
        }

        private void OnDestroy()
        {
            playerInput.Join -= OnJoin;
        }

        private void OnJoin()
        {
            if(currentPlayerCount < maxPlayers)
            {
                OnPlayerJoined?.Invoke(++currentPlayerCount);
            }
            else
            {
                Debug.Log($"Max players of {maxPlayers} reached");
            }
        }
    }
}