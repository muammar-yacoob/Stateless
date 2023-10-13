using SparkCore.Runtime.Core;
using UnityEngine;

public class ZombieDamage : InjectableMonoBehaviour
{
    public void TakeDamage(int damageAmount)
    {
        Debug.Log($"Outch {damageAmount}!");
    }
}