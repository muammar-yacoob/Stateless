using SparkCore.Runtime.Core;
using Stateless.Player.Events;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Stateless.Player
{
    public class PlayerSpawner : InjectableMonoBehaviour, IPlayerSpawner
    {
        [SerializeField] private GameObject[] playerPrefabs;
        [SerializeField] private GameObject[] spawnPoints;
        [SerializeField] private PlayerInputManager inputManager;

        protected override void Awake()
        {
            inputManager.onPlayerJoined += SpawnPlayer;
        }

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
            
            PlayersStatsManager.Instance.AddPlayer(new PlayerStats(playerIndex,100,0));
            PublishEvent(new PlayerSpawned(playerInstance, playerIndex));
        }
    }
}