using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    // for damaging player
    void Damage(float damageAmount);

    // for damaging enemy
    void Damage(float damageAmount, float kBForce, Vector2 kBAngle);
}
