using System;
using System.Linq;
using SparkCore.Runtime.Core;
using Stateless.Player.Events;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;

namespace Stateless.Player
{
    public class PlayerSpawner : InjectableMonoBehaviour, IPlayerSpawner
    {
        [SerializeField] private GameObject[] playerPrefabs;
        [SerializeField] private GameObject[] spawnPoints;
        [SerializeField] private PlayerInputManager inputManager;
        [Inject] private IPlayerStatsProvider playerStatsProvider;

        public static Action<int> PlayerSpawned;
        protected override void Awake()
        {
            CleanupSceneObjects();
            base.Awake();
            inputManager.onPlayerJoined += SpawnPlayer;
        }
        private void CleanupSceneObjects() => FindObjectsOfType<PlayerMovement>().ToList().ForEach(p => Destroy(p.gameObject));

        private void OnDestroy() => inputManager.onPlayerJoined -= SpawnPlayer;

        public void SpawnPlayer(PlayerInput playerInput)
        {
            int playerIndex = playerInput.playerIndex;
            Transform spawnPoint = spawnPoints[playerIndex % spawnPoints.Length].transform;
    
            GameObject playerInstance = Instantiate(
                playerPrefabs[playerIndex % playerPrefabs.Length],
                spawnPoint.position,
                spawnPoint.rotation,
                transform
            );

            playerInstance.name = $"Player {playerIndex + 1}";
            Selection.activeGameObject = playerInstance;

            var playerStats = new PlayerStats(playerInstance, playerIndex, 100, 0);
            playerStatsProvider.AddPlayer(playerStats);
            PlayerSpawned?.Invoke(playerIndex);
            PublishEvent(new PlayerSpawned(playerStats));
        }
    }
}