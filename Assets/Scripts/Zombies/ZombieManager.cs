using System;
using System.Collections.Generic;
using UnityEngine;

namespace Stateless.Zombies
{
    public class ZombieManager
    {
        private static readonly Lazy<ZombieManager> lazyInstance = new(() => new ZombieManager());
        public static ZombieManager Instance => lazyInstance.Value;

        private readonly List<Zombie> zombies = new List<Zombie>();

        public IReadOnlyList<Zombie> Zombies => zombies;

        public void RegisterZombie(Zombie zombie)
        {
            if (!zombies.Contains(zombie))
            {
                zombies.Add(zombie);
            }
        }

        public void UnregisterZombie(Zombie zombie)
        {
            zombies.Remove(zombie);
        }

        public Zombie FindClosestZombie(Vector3 myPosition)
        {
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
    }
}