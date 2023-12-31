using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    public void Damage(float damageAmount, Vector2 attackDirection);

    public bool HasTakenDamage { get; set; }
}
