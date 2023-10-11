using SparkCore.Runtime.Core;
using Stateless.Candy.Events;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]
public class CandyBullet : InjectableMonoBehaviour
{
    [SerializeField] private int fireCost = 1;
    [SerializeField] private int damageAmount = 1;
    [SerializeField] private ParticleSystem particleFX;
    [SerializeField] private AudioClip audioClip;
    private Rigidbody rb;

    protected override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody>();
        //rb.isKinematic = true;

        var boxCollider = GetComponent<BoxCollider>();
        boxCollider.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<ZombieDamage>(out var zombieDamage))
        {
            zombieDamage.TakeDamage(damageAmount);
            PublishEvent(new CandyFired(fireCost));
            Destroy(gameObject);
        }
    }
}