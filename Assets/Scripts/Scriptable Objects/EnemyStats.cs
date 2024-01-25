using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "EnemyStats")]
public class EnemyStats : ScriptableObject
{
    [Header("Enemy Variables")]
    public float maxHealth;
    public float enemySpeed;
    public float playerDetectPauseTime;
    public float playerDetectedWaitTime;
    public float dashTime;
    public float dashSpeed;

    [Header("Check Distance")]
    public float groundDistance;
    public float obstacleDistance;
    public float playerDetectDistance;
    public float meleeAttackDistance;

    [Header("Damage")]
    public float eDamageAmount;
    public Vector2 knockbackAngle;
    public float knockbackForce;
}
