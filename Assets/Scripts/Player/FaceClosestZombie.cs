using Stateless.Zombies;
using UnityEngine;

namespace Stateless.Player
{
    public class FaceClosestZombie : MonoBehaviour
    {
        [SerializeField] private float rotationSpeed = 5f;

        void Update()
        {
            Zombie closestZombie = FindClosestZombie();

            if (closestZombie != null)
            {
                RotateTowards(closestZombie.transform);
                Debug.DrawRay(transform.position, (closestZombie.transform.position - transform.position).normalized * 10, Color.green);
            }
            Debug.DrawRay(transform.position, transform.forward * 5, Color.red);
        }

        Zombie FindClosestZombie()
        {
            var zombies = ZombieManager.Instance.Zombies;
            Zombie closestZombie = null;
            float closestDistance = float.MaxValue;

            foreach (var zombie in zombies)
            {
                if (zombie == null || zombie.IsDying) continue;

                float distanceToZombie = Vector3.Distance(transform.position, zombie.transform.position);

                if (distanceToZombie < closestDistance)
                {
                    closestZombie = zombie;
                    closestDistance = distanceToZombie;
                }
            }

            return closestZombie;
        }

        void RotateTowards(Transform target)
        {
            Vector3 directionToZombie = (target.position - transform.position).normalized;
            directionToZombie.y = 0; // Ignore y-axis for direction
            Quaternion targetRotation = Quaternion.LookRotation(directionToZombie);

            // Get only the y-component for the new rotation
            Quaternion finalRotation = Quaternion.Euler(0, targetRotation.eulerAngles.y, 0);

            transform.rotation = Quaternion.Slerp(transform.rotation, finalRotation, Time.deltaTime * rotationSpeed);
        }
    }
}