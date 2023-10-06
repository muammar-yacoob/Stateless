using System;

namespace Stateless.Zombies.Events
{
    public class ZombieSpawned
    {
        public readonly Zombie ZombieInstance;

        public ZombieSpawned(Zombie zombieInstance)
        {
            ZombieInstance = zombieInstance;
        }
    }
}