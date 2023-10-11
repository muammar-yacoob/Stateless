using System;
using SparkCore.Runtime.Core;
using Stateless.Candy.Events;
using Stateless.Zombies;
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
        rb.isKinematic = true;

        var boxCollider = GetComponent<BoxCollider>();
        boxCollider.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(typeof(ZombieDamage), out var zombie))
        {
            zombie(damageAmount);
            PublishEvent(new CandyFired(fireCost));
        }
    }
}

public class ZombieDamage : InjectableMonoBehaviour
{
    public void TakeDamage(int damageAmount)
    {
        Debug.Log($"Outch {damageAmount}!");
    }
}