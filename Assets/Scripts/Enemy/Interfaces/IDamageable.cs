using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    // For player
    public void PDamage(float damageAmount, Vector2 attackDirection);
    
    //For enemy
    public void EDamage(float eDamageAmount);

    public bool HasTakenDamage { get; set; }
}
