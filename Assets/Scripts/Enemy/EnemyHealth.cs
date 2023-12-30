using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    [SerializeField] private float maxHealth;
    private float currentHealth;
    public bool HasTakenDamage { get ; set ; }
    private CinemachineImpulseSource impulseSource;

    private void Start()
    {
        currentHealth = maxHealth;

        impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    public void Damage(float damageAmount)
    {
        CameraShakeManager.instance.CameraShake(impulseSource);
        HasTakenDamage = true;
        currentHealth -= damageAmount;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die() 
    {
        Destroy(this.gameObject);
    }
}
