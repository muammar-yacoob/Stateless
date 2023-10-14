using SparkCore.Runtime.Core;
using UnityEngine;

namespace Stateless.Zombies
{
    public class ZombieDamage : InjectableMonoBehaviour, IDamageable
    {
        private Zombie zombie;
        protected override void Awake()
        {
            base.Awake();
            zombie = GetComponent<Zombie>();
        }

        public void TakeDamage(int damageAmount)
        {
            Debug.Log($"Zombie took {damageAmount} damage");
            zombie.IsDying = true;
            Destroy(gameObject);
        }
    }
}