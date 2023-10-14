using Stateless.Zombies;
using UnityEngine;

namespace Stateless.Player
{
    public class FaceClosestZombie : MonoBehaviour
    {
        [SerializeField] private float rotationSpeed = 5f;
        [SerializeField] Transform firePoint;

        void Update()
        {
            Zombie closestZombie = ZombieManager.Instance.FindClosestZombie(firePoint.position);

            if (closestZombie != null)
            {
                RotateTowards(closestZombie.transform);
            }
            else if (firePoint.rotation != Quaternion.identity)
            {
                firePoint.localRotation = Quaternion.identity;
            }
        }

        void RotateTowards(Transform target)
        {
            Vector3 directionToZombie = (target.position - firePoint.position).normalized;
            directionToZombie.y = 0;

            Quaternion targetRotation = Quaternion.LookRotation(directionToZombie);
            Quaternion finalRotation = Quaternion.Euler(0, targetRotation.eulerAngles.y, 0);

            firePoint.rotation = Quaternion.Slerp(firePoint.rotation, finalRotation, Time.deltaTime * rotationSpeed);
        }
    }
}