using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class NewBehaviourScript : MonoBehaviour, IDamageable
{
    private CinemachineImpulseSource impulseSource;
    private ParticleSystem damageParticlesInstance;
    [SerializeField] private ScreenShakeProfile profile;
    [SerializeField] private float maxHealth;
    [SerializeField] private ParticleSystem damageParticles;
    private float currentHealth;
    public bool HasTakenDamage { get; set; }

    private void Start()
    {
        currentHealth = maxHealth;

        impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    public void Damage(float damageAmount, Vector2 attackDirection)
    {
        CameraShakeManager.instance.ScreenShakeFromProfile(profile, impulseSource);
        HasTakenDamage = true;
        currentHealth -= damageAmount;
        //spawn particles
        SpawnDamageParticles(attackDirection);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }

    private void SpawnDamageParticles(Vector2 attackDirection)
    {
        Quaternion spawnRotation = Quaternion.FromToRotation(Vector2.right, attackDirection);
        damageParticlesInstance = Instantiate(damageParticles, transform.position, spawnRotation);
    }
}
