using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    // call when player is damaging the enemy
    public void PDamage(float damageAmount, Vector2 attackDirection, float KBForce, Vector2 KBAngle);

    // call when enemy is damaging the player
    public void EDamage(float eDamageAmount);

    public bool HasTakenDamage { get; set; }
}
