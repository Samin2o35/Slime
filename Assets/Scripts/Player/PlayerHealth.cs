using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    public float health;
    public Animator hitPraticleAnim;

    private void Update()
    {
        if(health <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void Damage(float damageAmount)
    {
        hitPraticleAnim.Play("Enemy - HitParticle");
        Debug.Log("hit");
        health -= damageAmount;
    }
}
