using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemyDamageable
{
    public void EnemyDamage(float eDamageAmount);

    public bool HasTakenDamage { get; set; }
}
