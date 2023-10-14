using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Cysharp.Threading.Tasks;
using SparkCore.Runtime.Core;
using Stateless.Player;
using Stateless.Player.Events;
using Stateless.Zombies.Events;

namespace Stateless.Zombies
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class Zombie : InjectableMonoBehaviour
    {
        [Header("Zombie Attributes")]
        [SerializeField] private float sightRange = 10f;
        [SerializeField] private float sightAngle = 120f;
        [SerializeField] private float damage = 10f;

        [Header("Cooldown and Delay Times (ms)")]
        [SerializeField] private int attackCooldown = 1000; // in milliseconds
        [SerializeField] private int roamCooldown = 5000; // in milliseconds
        [SerializeField] private int attackDelay = 10000; // in milliseconds

        [Header("Roaming Area")]
        [SerializeField] private float roamMinX = -20f;
        [SerializeField] private float roamMaxX = 20f;
        [SerializeField] private float roamMinZ = -20f;
        [SerializeField] private float roamMaxZ = 20f;

        private NavMeshAgent navAgent;
        private List<PlayerStats> players;

        private bool isAttacking = false;
        private bool inCooldown = false;
        public bool IsDying;

        private void Start()
        {
            navAgent = GetComponent<NavMeshAgent>();
            SubscribeEvent<PlayerSpawned>(OnPlayerSpawned);
            players = PlayersStatsManager.Instance.GetPlayers();
            RoamToRandomLocation().Forget();
        }

        private void OnPlayerSpawned(PlayerSpawned playerSpawned)
        {
            players = PlayersStatsManager.Instance.GetPlayers();
        }

        private void OnDestroy() => UnsubscribeEvent<PlayerSpawned>(OnPlayerSpawned);

        private void Update()
        {
            if(IsDying) return; //Zombie has been destroyed

            if (players.Count == 0) return;
            var targetPlayer = FindClosestPlayerInSight();
            if (targetPlayer != null && !isAttacking)
            {
                StopRoaming();
                ChaseAndAttack(targetPlayer).Forget();
            }
        }

        private PlayerStats FindClosestPlayerInSight()
        {
            PlayerStats closestPlayer = null;
            float closestDistance = sightRange;

            foreach (var player in players)
            {
                Transform playerTransform = player.PlayerInstance.transform;
                Vector3 directionToPlayer = (playerTransform.position - transform.position).normalized;
                float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
                float angle = Vector3.Angle(transform.forward, directionToPlayer);
                Debug.DrawRay(transform.position, directionToPlayer * sightRange, Color.red); 

                if (distanceToPlayer <= sightRange && angle < sightAngle * 0.5f)
                {
                    Debug.DrawRay(transform.position, directionToPlayer * sightRange, Color.green); 

                    if (distanceToPlayer < closestDistance)
                    {
                        closestPlayer = player;
                        closestDistance = distanceToPlayer;
                    }
                }
            }
            return closestPlayer;
        }

        private async UniTaskVoid ChaseAndAttack(PlayerStats targetPlayer)
        {
            if(IsDying) return; //Zombie has been destroyed
            isAttacking = true;
            Transform playerTransform = targetPlayer.PlayerInstance.transform;

            while (IsPlayerInSight(playerTransform))
            {
                navAgent.SetDestination(playerTransform.position);
                if (navAgent.remainingDistance <= navAgent.stoppingDistance)
                {
                    PublishEvent(new PlayerDamaged(damage, targetPlayer.PlayerIndex));
                    await UniTask.Delay(attackCooldown);
                }
                await UniTask.Yield();
            }
            isAttacking = false;
            inCooldown = true;
            await UniTask.Delay(attackDelay);
            inCooldown = false;
            RoamToRandomLocation().Forget();
        }

        private bool IsPlayerInSight(Transform playerTransform)
        {
            if(IsDying) return false; //Zombie has been destroyed
            if (playerTransform == null) return false;
            Vector3 directionToPlayer = (playerTransform.position - transform.position).normalized;
            float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
            float angle = Vector3.Angle(transform.forward, directionToPlayer);
            return distanceToPlayer <= sightRange && angle < sightAngle * 0.5f;
        }

        private async UniTaskVoid RoamToRandomLocation()
        {
            while (!isAttacking && !inCooldown)
            {
                Vector3 randomPosition = new Vector3(
                    UnityEngine.Random.Range(roamMinX, roamMaxX),
                    0,
                    UnityEngine.Random.Range(roamMinZ, roamMaxZ)
                );

                if(navAgent == null) return; //Zombie has been destroyed
                navAgent.SetDestination(randomPosition);
                await UniTask.Delay(roamCooldown);
            }
        }

        private void StopRoaming()
        {
            navAgent.isStopped = true;
            navAgent.isStopped = false;
        }
    }
}