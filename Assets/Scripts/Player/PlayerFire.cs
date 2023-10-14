using SparkCore.Runtime.Core;
using Stateless.Fire;
using UnityEngine;

namespace Stateless.Player
{
    public class PlayerFire : InjectableMonoBehaviour
    {
        [SerializeField] private Transform firePoint;
        [SerializeField] private GameObjectPool pool;

        protected override void Awake()
        {
            pool.Prewarm();
        }

        public void Fire()
        {
            var bullet = pool.Get();
            bullet.transform.position = firePoint.position;
            bullet.transform.rotation = firePoint.rotation;
        }
    }
}