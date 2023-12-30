using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class NewBehaviourScript : MonoBehaviour, IDamageable
{
    private CinemachineImpulseSource impulseSource;
    [SerializeField] private ScreenShakeProfile profile;
    [SerializeField] private float maxHealth;
    private float currentHealth;
    public bool HasTakenDamage { get; set; }

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
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
