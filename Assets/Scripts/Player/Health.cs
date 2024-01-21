using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour, IEnemyDamageable
{
    [Header("Health")]
    [SerializeField] private float maxHealth;
    private HealthBar healthBar;
    private float currentHealth;
    public bool HasTakenDamage { get; set; }

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void EnemyDamage(float eDamageAmount)
    {
        //CameraShakeManager.instance.ScreenShakeFromProfile(profile, impulseSource);
        HasTakenDamage = true;
        currentHealth -= eDamageAmount;

        //update health bar according to damage taken
        healthBar.UpdateHealthBar(maxHealth, currentHealth);

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
