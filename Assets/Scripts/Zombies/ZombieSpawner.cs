using System.Linq;
using Cysharp.Threading.Tasks;
using SparkCore.Runtime.Core;
using Stateless.Player.Events;
using Stateless.Zombies.Events;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

namespace Stateless.Zombies
{
    public class ZombieSpawner : InjectableMonoBehaviour
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

        protected override void Awake()
        {
            CleanupSceneObjects();
            SubscribeEvent<PlayerSpawned>(OnPlayerSpawned);
        }
        private void OnDestroy() => UnsubscribeEvent<PlayerSpawned>(OnPlayerSpawned);

        private void OnPlayerSpawned(PlayerSpawned obj) => SpawnZombiesPeriodically().Forget();
        private void CleanupSceneObjects() => FindObjectsOfType<Zombie>().ToList().ForEach(z => z.gameObject.SetActive(false));

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

            PublishEvent<ZombieSpawned>(new ZombieSpawned(newZombie));
        }
    }
}