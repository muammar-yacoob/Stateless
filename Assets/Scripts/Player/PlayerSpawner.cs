using SparkCore.Runtime.Injection;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    [RuntimeObject(RuntimeObjectType.Singleton)]
    public class PlayerSpawner : MonoBehaviour, IPlayerSpawner
    {
        [SerializeField] private GameObject[] playerPrefabs;
        [SerializeField] private GameObject[] spawnPoints;
        private PlayerInputManager inputManager;

        private void Awake()
        {
            inputManager = FindObjectOfType<PlayerInputManager>();
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
        }
    }
}