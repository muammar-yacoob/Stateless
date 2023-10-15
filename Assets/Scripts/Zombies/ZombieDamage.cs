using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using SparkCore.Runtime.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Stateless.Zombies
{
    [RequireComponent(typeof(Animator))]
    public class ZombieDamage : InjectableMonoBehaviour, IDamageable
    {
        [SerializeField] private Image healthFill;
        [SerializeField] private int health = 10;
        [SerializeField] private ParticleSystem deathParticles;
        [SerializeField] private ParticleSystem hitParticles;
        
        private static readonly int Damage = Animator.StringToHash("Hit");
        private static readonly int Fall = Animator.StringToHash("Fall");
        
        private Zombie zombie;
        private Animator anim;

        protected override void Awake()
        {
            base.Awake();
            zombie = GetComponent<Zombie>();
            anim = GetComponent<Animator>();
        }

        public void TakeDamage(int damageAmount)
        {
            Debug.Log($"Zombie took {damageAmount} damage");
            health -= damageAmount;
            healthFill.fillAmount = (float)health / 10f;
            hitParticles.Play();
            anim.CrossFade(Damage, 0.1f);
            
            if (health <= 0)
            {
                _ = Die();
            }
        }

        private async UniTask Die()
        {
            zombie.IsDying = true;
            anim.CrossFade(Fall, 0.1f);
            await UniTask.Delay(1000);
            deathParticles.Play();
            Destroy(gameObject);
        }
    }
}