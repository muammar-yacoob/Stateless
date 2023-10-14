using SparkCore.Runtime.Core;
using Stateless.Fire;
using Stateless.Zombies;
using UnityEngine;

namespace Stateless.Player
{
    public class PlayerFire : InjectableMonoBehaviour
    {
        [SerializeField] private Transform firePoint;
        [SerializeField] private GameObjectPool pool;
        [SerializeField] private float maxRotationAngle = 120f;
        [SerializeField] private float maxDistance = 10f;

        protected override void Awake()
        {
            pool.Prewarm();
        }

        public void Fire()
        {
            Zombie closestZombie = ZombieManager.Instance.FindClosestZombie(firePoint.position);

            if (closestZombie != null)
            {
                float angle = Vector3.Angle(firePoint.forward, (closestZombie.transform.position - firePoint.position).normalized);
                float distance = Vector3.Distance(firePoint.position, closestZombie.transform.position);
                Debug.DrawRay(firePoint.position, (closestZombie.transform.position - firePoint.position).normalized * distance, Color.red,2f);

                if (angle <= maxRotationAngle && distance <= maxDistance)
                {
                    Debug.DrawRay(firePoint.position, (closestZombie.transform.position - firePoint.position).normalized * distance, Color.green,2f);
                    var bullet = pool.Get();
                    bullet.transform.position = firePoint.position;
                    bullet.transform.rotation = firePoint.rotation;
                }
            }
        }
    }
}