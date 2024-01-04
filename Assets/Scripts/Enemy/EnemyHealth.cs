using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    private CinemachineImpulseSource impulseSource;
    private ParticleSystem damageParticlesInstance;
    private HealthBar _healthBar;

    [SerializeField] private ScreenShakeProfile profile;
    [SerializeField] private float maxHealth;
    [SerializeField] private ParticleSystem damageParticles;
    private float currentHealth;
    public bool HasTakenDamage { get ; set ; }

    private void Start()
    {
        currentHealth = maxHealth;
        _healthBar = GetComponentInChildren<HealthBar>();
        impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    public void Damage(float damageAmount, Vector2 attackDirection)
    {
        CameraShakeManager.instance.ScreenShakeFromProfile(profile, impulseSource);
        HasTakenDamage = true;
        currentHealth -= damageAmount;
        
        //spawn particles
        SpawnDamageParticles(attackDirection);
        
        //update health bar according to damage taken
        _healthBar.UpdateHealthBar(maxHealth, currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die() 
    {
        Destroy(this.gameObject);
    }

    private void SpawnDamageParticles(Vector2 attackDirection)
    {
        Quaternion spawnRotation = Quaternion.FromToRotation(Vector2.right, attackDirection);
        damageParticlesInstance = Instantiate(damageParticles, transform.position, spawnRotation);
    }
}
