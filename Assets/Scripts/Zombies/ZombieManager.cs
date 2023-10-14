using System;
using System.Collections.Generic;

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
    }
}