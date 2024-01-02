using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    private CinemachineImpulseSource impulseSource;
    private ParticleSystem damageParticlesInstance;
    [SerializeField] private ScreenShakeProfile profile;
    [SerializeField] private float maxHealth;
    [SerializeField] private ParticleSystem damageParticles;
    private float currentHealth;
    public bool HasTakenDamage { get ; set ; }

    private void Start()
    {
        currentHealth = maxHealth;

        impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    public void Damage(float damageAmount)
    {
        CameraShakeManager.instance.ScreenShakeFromProfile(profile, impulseSource);
        HasTakenDamage = true;
        currentHealth -= damageAmount;
        //spawn particles
        SpawnDamageParticles();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die() 
    {
        Destroy(this.gameObject);
    }

    private void SpawnDamageParticles()
    {
        damageParticlesInstance = Instantiate(damageParticles, transform.position, Quaternion.identity);
    }
}
