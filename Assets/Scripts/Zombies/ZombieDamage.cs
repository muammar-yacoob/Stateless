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
        
        private static readonly int Hit = Animator.StringToHash("Hit");
        private static readonly int Die1 = Animator.StringToHash("Die");
        
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
            anim.SetTrigger(Hit);
            
            if (health <= 0) Die();
        }

        private async UniTask Die()
        {
            zombie.IsDying = true;
            anim.SetTrigger(Die1);
            await UniTask.Delay(1000);
            deathParticles.Play();
            Destroy(gameObject);
        }
    }
}