using SparkCore.Runtime.Core;
using Stateless.Player;
using UnityEngine;
using UnityEngine.Serialization;
using VContainer;

namespace Stateless.Candy
{
    public class PlayerFire : InjectableMonoBehaviour
    {
        [Header("Projectile")] [SerializeField]
        private Transform throwPoint;

        [SerializeField] private CandyBullet candyPrefab;
        [SerializeField] private LineRenderer lineRenderer;

        [FormerlySerializedAs("projectileSpeed")] [SerializeField]
        private float fireForce = 50f;

        [Header("Damage")] [SerializeField] private LayerMask zombieLayer;
        [SerializeField] private int damageAmount = 1;
        [SerializeField] private int fireCost = 1;
        [SerializeField] private float fireAndgle = 120f;

        [Inject] private IPlayerStatsProvider playerStatsProvider;
        private bool isWithinFireRange;

        protected override void Awake()
        {
            base.Awake();
            if (throwPoint == null) Debug.LogError("Throw point unassigned");
            if (lineRenderer == null) Debug.LogError("Line renderer unassigned");

            var currentCandyCount = playerStatsProvider.PlayerStats[0].Candies;
            Debug.Log($"Current candy count:{currentCandyCount}");
        }

        private void Update() => Aim();

        private void Aim()
        {
            RaycastHit hit;
            Debug.DrawRay(transform.position, throwPoint.forward, Color.blue);
            if (Physics.Raycast(throwPoint.position, throwPoint.forward, out hit, Mathf.Infinity, zombieLayer))
            {
                float angle = Vector3.Angle(throwPoint.forward, hit.point - throwPoint.position);
                if (angle < fireAndgle / 2)
                {
                    DrawLineToZombie(hit.point);
                    isWithinFireRange = true;
                }
            }
        }

        private void DrawLineToZombie(Vector3 zombiePosition)
        {
            lineRenderer.SetPosition(0, throwPoint.position);
            lineRenderer.SetPosition(1, zombiePosition);
        }

        public void Fire()
        {
            if (!isWithinFireRange) return;

            var candyInstance = Instantiate(candyPrefab, throwPoint.position, throwPoint.rotation);
            var rb = candyInstance.GetComponent<Rigidbody>();
            rb.AddForce(throwPoint.forward * fireForce);
        }
    }
}