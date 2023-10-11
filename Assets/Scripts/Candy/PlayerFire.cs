using System.Collections.Generic;
using SparkCore.Runtime.Core;
using Stateless.Player;
using UnityEngine;
using VContainer;

namespace Stateless.Candy
{
    [RequireComponent(typeof(LineRenderer))]
    public class PlayerFire : InjectableMonoBehaviour
    {
        [Header("Projectile")] 
        [SerializeField] private Transform firePoint;
        [SerializeField] private List<CandyBullet> candyPrefabs;
        [SerializeField] private float fireForce = 50f;

        [Header("Damage")]
        [SerializeField] private LayerMask zombieLayer;
        [SerializeField] private int damageAmount = 1;
        [SerializeField] private int fireCost = 1;
        [SerializeField] private float fireAndgle = 120f;

        [Inject] private IPlayerStatsProvider playerStatsProvider;
        private LineRenderer lineRenderer;
        private bool isWithinFireRange;

        private void Start()
        {
            if (firePoint == null) Debug.LogError("Throw point unassigned");
            lineRenderer = GetComponent<LineRenderer>();

            var currentCandyCount = playerStatsProvider.PlayerStats[0].Candies;
            Debug.Log($"Current candy count:{currentCandyCount}");
        }

        private void Update() => Aim();

        private void Aim()
        {
            RaycastHit hit;
            Debug.DrawRay(firePoint.position, firePoint.forward, Color.blue);
            if (Physics.Raycast(firePoint.position, firePoint.forward, out hit, Mathf.Infinity, zombieLayer))
            {
                float angle = Vector3.Angle(firePoint.forward, hit.point - firePoint.position);
                if (angle < fireAndgle / 2)
                {
                    DrawLineToZombie(hit.point);
                    isWithinFireRange = true;
                }
            }
        }

        private void DrawLineToZombie(Vector3 zombiePosition)
        {
            Debug.DrawRay(firePoint.position, firePoint.forward, Color.green);
            lineRenderer.enabled = true;
            lineRenderer.SetPosition(0, firePoint.position);
            lineRenderer.SetPosition(1, zombiePosition);
        }

        public void Fire()
        {
            Debug.Log($"Fire()");
            // if (!isWithinFireRange) return;
            // if (playerStatsProvider.PlayerStats[0].Candies < fireCost) return;
            // playerStatsProvider.PlayerStats[0].Candies -= fireCost;
            var candyPrefab = GetCandyPrefab();
            var candyInstance = Instantiate(candyPrefab, firePoint.position, firePoint.rotation);
            var rb = candyInstance.GetComponent<Rigidbody>();
            rb.velocity = Vector3.zero;
            rb.AddForce(firePoint.forward * fireForce, ForceMode.Impulse);
        }

        private CandyBullet GetCandyPrefab()
        {
            int randomIndex = Random.Range(0, candyPrefabs.Count);
            return candyPrefabs[randomIndex];
        }
    }
}