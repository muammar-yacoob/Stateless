using SparkCore.Runtime.Core;
using Stateless.Player.Events;
using Stateless.Zombies;
using UnityEngine;
using UnityEngine.Serialization;

namespace Stateless.Fire
{
    public class Bullet_3D : InjectableMonoBehaviour, IGameObjectPooled //implement interface
    {
        [SerializeField] private float LaunchSpeed = 20;
        [SerializeField] [Tooltip("In Seconds")] private float maxLifeTime = 3f;
        [SerializeField] [Tooltip("Typically the enemy's layer")] private LayerMask damagableLayer;
        [SerializeField] ParticleSystem particleOnCollision;
        [SerializeField] AudioClip sfxOnFire;
        [SerializeField] int damageAmount = 1;

        private float lifeTime;

        public GameObjectPool Pool { get; set; } //implement interface
        private void OnEnable()
        {
            lifeTime = 0;
            if (sfxOnFire != null)
            {
                PublishEvent(new BulletFired(sfxOnFire, transform.position));
            }
            if(particleOnCollision != null)
            {
                particleOnCollision.Stop();
                particleOnCollision.gameObject.SetActive(false);
            }
        }

        void Update()
        {
            transform.Translate(Vector3.forward * LaunchSpeed * Time.deltaTime);
            lifeTime += Time.deltaTime;

            if (lifeTime > maxLifeTime)
                Pool?.ReturnToPool(gameObject); //return to pool instead of destroy
        }
        private void OnCollisionEnter(Collision collision)
        {
            if ((damagableLayer & 1 << collision.gameObject.layer) != 1 << collision.gameObject.layer)
                return;

            //Deal Damage
            if(collision.gameObject.TryGetComponent(out IDamageable zombieDamage))
            {
                zombieDamage.TakeDamage(damageAmount);
            }
            
            //Play Effects
            if (particleOnCollision == null)
            {
                Pool?.ReturnToPool(gameObject);
            }
            else
            {
                particleOnCollision.gameObject.SetActive(false);
                particleOnCollision.gameObject.SetActive(true);
                particleOnCollision.Stop();
                particleOnCollision.Play();
                Pool?.ReturnToPool(gameObject, particleOnCollision.main.duration);
            }
        }
    }


}