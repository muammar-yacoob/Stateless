using System;
using Stateless.Zombies;

namespace GameEvents
{
    public class ZombieEvents
    {
        public static ZombieEvents Instance { get; } = new ZombieEvents();
        public event Action<Zombie> ZombieSpawned;
        public void SpawnZombie(Zombie zombie) => ZombieSpawned?.Invoke(zombie);
    }
}