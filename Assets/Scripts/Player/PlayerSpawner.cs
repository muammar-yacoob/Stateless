using SparkCore.Runtime.Injection;
using UnityEditor;
using UnityEngine;

namespace Player
{
    [RuntimeObject(RuntimeObjectType.Singleton)]
    public class PlayerSpawner : MonoBehaviour, IPlayerSpawner
    {
        [SerializeField] private GameObject[] playerPrefabs;

        private void Awake()
        {
            PlayersJoining.OnPlayerJoined += SpawnPlayer;
        }

        public void SpawnPlayer(int playerIndex)
        {
            var adjustedIndex = playerIndex - 1;
            var playerInstance = Instantiate(playerPrefabs[adjustedIndex % playerPrefabs.Length]);
            playerInstance.name = $"Player {playerIndex}";
            playerInstance.transform.parent = transform;
            Selection.activeGameObject = playerInstance;
        }
    }
}