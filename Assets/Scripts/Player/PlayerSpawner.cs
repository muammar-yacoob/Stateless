using SparkCore.Runtime.Injection;
using UnityEditor;
using UnityEngine;

namespace Player
{
    [RuntimeObject(RuntimeObjectType.Singleton)]
    public class PlayerSpawner : MonoBehaviour, IPlayerSpawner
    {
        [SerializeField] private GameObject[] playerPrefabs;
        [SerializeField] private GameObject[] spawnPoints;

        private void Awake()
        {
            PlayersJoining.OnPlayerJoined += SpawnPlayer;
        }

        public void SpawnPlayer(int playerIndex)
        {
            int adjustedIndex = playerIndex - 1;
            Transform spawnPoint = spawnPoints[adjustedIndex % spawnPoints.Length].transform;
    
            GameObject playerInstance = Instantiate(
                playerPrefabs[adjustedIndex % playerPrefabs.Length],
                spawnPoint.position,
                spawnPoint.rotation,
                transform
            );

            playerInstance.name = $"Player {playerIndex}";
            Selection.activeGameObject = playerInstance;
        }
    }
}