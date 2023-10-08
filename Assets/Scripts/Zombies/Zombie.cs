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
        [SerializeField] private float sightRange = 20f;
        [SerializeField] private float sightAngle = 120f;
        [SerializeField] private float damage = 10f;
        [SerializeField] private LayerMask playerLayer;

        private NavMeshAgent navAgent;
        private List<PlayerStats> players;

        private bool isAttacking;
        private bool inCooldown;

        private void Start()
        {
            base.Awake();
            navAgent = GetComponent<NavMeshAgent>();
            SubscribeEvent<PlayerSpawned>(OnPlayerSpawned);
            RoamToRandomLocation();
        }
        private void OnDestroy() => UnsubscribeEvent<PlayerSpawned>(OnPlayerSpawned);

        private void OnPlayerSpawned(PlayerSpawned player)
        {
            players = PlayersStatsManager.Instance.GetPlayers();
            Debug.Log($"{players.Count} players found");
        }

        private void Update()
        {
            if(players == null || players.Count == 0)
            {
                players = PlayersStatsManager.Instance.GetPlayers();
                return;
            }
            // if(inCooldown) return;
            // if(isAttacking) return;
            // if(navAgent.isStopped) return;
            // if(navAgent.pathPending) return;
            // if(navAgent.remainingDistance > navAgent.stoppingDistance) return;
            
            var targetPlayer = FindClosestPlayerInSight();
            if(targetPlayer == null) return;
            Debug.Log($"Target Player {targetPlayer.PlayerIndex}", targetPlayer.PlayerInstance);
            if (targetPlayer != null && !isAttacking)
            {
                //StopRoaming();
                Debug.Log($"Chasing {targetPlayer.PlayerIndex}");
                ChaseAndAttack(targetPlayer).Forget();
            }
        }

        private PlayerStats FindClosestPlayerInSight()
        {
            PlayerStats closestPlayer = null;
            float closestDistance = sightRange;

            foreach (var player in players)
            {
                if (player.PlayerInstance == null)
                {
                    Debug.LogWarning("Player instance is null.");
                    continue;
                }

                Transform playerTransform = player.PlayerInstance.transform;
                Vector3 directionToPlayer = (playerTransform.position - transform.position).normalized;
                float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
                float angle = Vector3.Angle(transform.forward, directionToPlayer);
                Debug.DrawLine(transform.position, playerTransform.position, Color.red);
                if (distanceToPlayer <= sightRange && angle < sightAngle * 0.5f)
                {
                    RaycastHit hit;
                    Debug.DrawLine(transform.position, playerTransform.position, Color.green);
                    if (Physics.Raycast(transform.position, directionToPlayer, out hit, sightRange, playerLayer))
                    {
                        if (hit.collider.gameObject == player.PlayerInstance)
                        {
                            if (distanceToPlayer < closestDistance)
                            {
                                closestPlayer = player;
                                closestDistance = distanceToPlayer;
                            }
                        }
                    }
                }
            }
            return closestPlayer;
        }


        private async UniTaskVoid ChaseAndAttack(PlayerStats targetPlayer)
        {
            if(targetPlayer == null) return;
            isAttacking = true;
            Transform playerTransform = targetPlayer.PlayerInstance.transform;

            while (IsPlayerInSight(playerTransform))
            {
                navAgent.SetDestination(playerTransform.position);
                if (navAgent.remainingDistance <= navAgent.stoppingDistance)
                {
                    //Debug.Log($"Attacking Player {targetPlayer.PlayerIndex} with {damage} damage");
                    PublishEvent(new PlayerDamaged(damage, targetPlayer.PlayerIndex));
                    await UniTask.Delay(1000);
                }
                //else Debug.Log($"Remaining: {navAgent.remainingDistance}, Stopping: {navAgent.stoppingDistance}");

                await UniTask.Yield();
            }
            isAttacking = false;
            inCooldown = true;
            await UniTask.Delay(10000);
            inCooldown = false;
            RoamToRandomLocation().Forget();
        }

        private bool IsPlayerInSight(Transform playerTransform)
        {
            if(playerTransform == null) return false;
            var zombiePosition = transform.position;
            var playerPosition = playerTransform.position;
            Vector3 directionToPlayer = (playerPosition - zombiePosition).normalized;
            float distanceToPlayer = Vector3.Distance(zombiePosition, playerPosition);
            float angle = Vector3.Angle(transform.forward, directionToPlayer);
            return distanceToPlayer <= sightRange && angle < sightAngle * 0.5f;
        }

        private async UniTaskVoid RoamToRandomLocation()
        {
            while (!isAttacking && !inCooldown)
            {
                if (!navAgent.isActiveAndEnabled || !navAgent.isOnNavMesh)
                {
                    Debug.LogError("Agent is either not active or not on a NavMesh. Cannot set destination.");
                    return;
                }

                Vector3 randomPosition = new Vector3(
                    Random.Range(-2, 2),
                    0,
                    Random.Range(-2, 2)
                );

                navAgent.SetDestination(randomPosition);
        
                await UniTask.Delay(1000);
            }
        }

        private void StopRoaming()
        {
            navAgent.isStopped = true;
            navAgent.ResetPath();
            navAgent.isStopped = false;
        }
    }
}