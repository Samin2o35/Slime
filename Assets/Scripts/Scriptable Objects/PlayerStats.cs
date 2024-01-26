using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PlayerStats")]
public class PlayerStats : ScriptableObject
{
    [Header("Player Variables")]
    public float attackRange;
    public float damageAmount;
    public float timeBetweenAttack;

    [Header("Health")]
    public float maxHealth;

    [Header("Enemy Knockback")]
    public float KBForce;
    public Vector2 KBAngle;
    
}
