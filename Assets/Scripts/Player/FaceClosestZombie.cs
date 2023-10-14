using Stateless.Zombies;
using UnityEngine;

namespace Stateless.Player
{
    public class FaceClosestZombie : MonoBehaviour
    {
        [SerializeField] private float rotationSpeed = 5f;
        [SerializeField] private float maxRotationAngle = 120f;
        [SerializeField] private float maxDistance = 10f;

        private Vector3 myPosition;

        void Update()
        {
            myPosition = transform.position; // Cache position for optimization
            Zombie closestZombie = FindClosestZombie();

            if (closestZombie != null)
            {
                RotateTowards(closestZombie.transform);
            }
            else if(transform.rotation != Quaternion.identity)
            {
                transform.localRotation = Quaternion.identity;
            }
        }

        Zombie FindClosestZombie()
        {
            var zombies = ZombieManager.Instance.Zombies;
            Zombie closestZombie = null;
            float closestDistance = float.MaxValue;

            foreach (var zombie in zombies)
            {
                if (zombie == null || zombie.IsDying) continue;

                float distanceToZombie = Vector3.Distance(myPosition, zombie.transform.position);

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
            Vector3 directionToZombie = (target.position - myPosition).normalized;
            directionToZombie.y = 0; // Ignore y-axis for direction

            Quaternion targetRotation = Quaternion.LookRotation(directionToZombie);
            Quaternion finalRotation = Quaternion.Euler(0, targetRotation.eulerAngles.y, 0);

            transform.rotation = Quaternion.Slerp(transform.rotation, finalRotation, Time.deltaTime * rotationSpeed);

            float angle = Vector3.Angle(transform.forward, directionToZombie);
            float distance = Vector3.Distance(myPosition, target.position);

            Color rayColor;

            if (angle <= maxRotationAngle && distance <= maxDistance)
            {
                rayColor = Color.green;
            }
            else
            {
                rayColor = Color.red;
            }
            
            Debug.DrawRay(myPosition, directionToZombie * 10, rayColor);
        }
    }
}
