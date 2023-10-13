using Stateless.Fire;
using UnityEngine;

namespace Stateless.Player
{
    public class PlayerFire : MonoBehaviour
    {
        [SerializeField] private Transform firePoint;
        [SerializeField] private GameObjectPool pool;
        [SerializeField] private AudioClip sFX;

        public void Fire()
        {
            var bullet = pool.Get();
            bullet.transform.position = firePoint.position;
            bullet.transform.rotation = firePoint.rotation;

            // if (sFX != null)
            // {
            //     OnFire?.Invoke(sFX);
            // }
        }
    }
}