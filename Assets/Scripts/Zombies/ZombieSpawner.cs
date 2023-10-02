using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

namespace Stateless.Zombies
{
    public class ZombieSpawner : MonoBehaviour
    {
        [Header("Spawning")]
        [SerializeField] private Zombie[] zombiePrefabs;
        [SerializeField] private GameObject[] spawnPoints;
        [Header("Difficulty")]
        [SerializeField] private int spawnDelay;
        [SerializeField] private int maxZombieCount;

        private int currentZombieCount;
        public int CurrentZombieCount => currentZombieCount;
        private PlayerInputManager inputManager;

        private void Awake()
        {
            inputManager = FindObjectOfType<PlayerInputManager>();
            inputManager.onPlayerJoined += StartSpawning;
            Cleanup();
        }

        private void OnDestroy() => inputManager.onPlayerJoined -= StartSpawning;
        private void StartSpawning(PlayerInput playerInput) => SpawnZombiesPeriodically().Forget();

        private void Cleanup() => FindObjectsOfType<Zombie>().ToList().ForEach(z => z.gameObject.SetActive(false));

        private async UniTaskVoid SpawnZombiesPeriodically()
        {
            while (true)
            {
                if (currentZombieCount < maxZombieCount)
                {
                    SpawnZombie();
                }
                await UniTask.Delay(spawnDelay*1000);
            }
        }

        private void SpawnZombie()
        {
            int index = Random.Range(0, zombiePrefabs.Length);
            Zombie selectedZombie = zombiePrefabs[index];

            int spawnPointIndex = Random.Range(0, spawnPoints.Length);
            GameObject selectedSpawnPoint = spawnPoints[spawnPointIndex];

            Zombie newZombie = Instantiate(selectedZombie, selectedSpawnPoint.transform.position, Quaternion.identity);
            newZombie.transform.parent = transform;
            currentZombieCount++;

            GameEvents.ZombieEvents.Instance.SpawnZombie(newZombie);
        }
    }
}