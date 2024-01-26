using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    [Header("Health")]
    [SerializeField] private float maxHealth;
    [SerializeField]private HealthBar healthBar;
    private float currentHealth;
    public bool HasTakenDamage { get; set; }

    private void Start()
    {
        currentHealth = maxHealth;
    }

    // call when player is damaging the enemy
    public void PDamage(float damageAmount, Vector2 attackDirection, float KBForce, Vector2 KBAngle)
    {

    }

    // call when enemy is damaging the player
    public void EDamage(float damageAmount)
    {
        //CameraShakeManager.instance.ScreenShakeFromProfile(profile, impulseSource);
        HasTakenDamage = true;
        currentHealth -= damageAmount;

        // update player health bar according to damage taken from enemy
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
