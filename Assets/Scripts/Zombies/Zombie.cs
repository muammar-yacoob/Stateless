using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using Cysharp.Threading.Tasks;
using Stateless.Player;
using UnityEngine.InputSystem;

namespace Stateless.Zombies
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class Zombie : MonoBehaviour
    {
        [SerializeField] private float sightRange = 10f;
        [SerializeField] private float sightAngle = 60f;
        [SerializeField] private float damage = 10f;

        private NavMeshAgent navAgent;
        private List<PlayerMovement> players;
        private PlayerInputManager inputManager;

        private bool isAttacking = false;
        private bool inCooldown = false;

        private void Start()
        {
            navAgent = GetComponent<NavMeshAgent>();
            inputManager = FindObjectOfType<PlayerInputManager>();
            FindPlayersInScene(null);
            inputManager.onPlayerJoined += FindPlayersInScene;
            RoamToRandomLocation().Forget();
        }

        private void OnDestroy()
        {
            if(inputManager != null)
                inputManager.onPlayerJoined -= FindPlayersInScene;
        }

        private void FindPlayersInScene(PlayerInput playerInput)
        {
            players = FindObjectsOfType<PlayerMovement>().ToList();
        }

        private void Update()
        {
            if (players.Count == 0) return;
            var targetPlayer = FindClosestPlayerInSight();
            if (targetPlayer != null && !isAttacking)
            {
                StopRoaming();
                ChaseAndAttack(targetPlayer).Forget();
            }
        }

        private PlayerMovement FindClosestPlayerInSight()
        {
            PlayerMovement closestPlayer = null;
            float closestDistance = sightRange;

            foreach (var player in players)
            {
                Transform playerTransform = player.transform;
                Vector3 directionToPlayer = (playerTransform.position - transform.position).normalized;
                float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
                float angle = Vector3.Angle(transform.forward, directionToPlayer);

                if (distanceToPlayer <= sightRange && angle < sightAngle * 0.5f)
                {
                    if (distanceToPlayer < closestDistance)
                    {
                        closestPlayer = player;
                        closestDistance = distanceToPlayer;
                    }
                }
            }
            return closestPlayer;
        }

        private async UniTaskVoid ChaseAndAttack(PlayerMovement targetPlayer)
        {
            isAttacking = true;
            var playerHealth = targetPlayer.GetComponent<PlayerHealth>();
            Transform playerTransform = targetPlayer.transform;

            while (IsPlayerInSight(playerTransform))
            {
                navAgent.SetDestination(playerTransform.position);
                if (navAgent.remainingDistance <= navAgent.stoppingDistance)
                {
                    playerHealth.TakeDamage(damage);
                    await UniTask.Delay(1000);
                }
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
                    UnityEngine.Random.Range(-20, 20),
                    0,
                    UnityEngine.Random.Range(-20, 20)
                );

                navAgent.SetDestination(randomPosition);
                await UniTask.Delay(5000);
            }
        }

        private void StopRoaming()
        {
            navAgent.isStopped = true;
            navAgent.isStopped = false;
        }
    }
}