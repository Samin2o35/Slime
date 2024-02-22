using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "EnemyStatsSO")]
public class EnemyStats : ScriptableObject
{
    [Header("General Stats")]
    public float maxHealth = 20;

    [Header("Prefabs")]
    public GameObject deathParticle;
    public GameObject[] deathDebris;
    
    [Header("Patrol State")]
    public float groundDistance;
    public float obstacleDistance;
    public float enemyMoveSpeed;

    [Header("Player Detection State")]
    public float playerDetectDistance;
    public float detectionPauseTime;
    public float playerDetectedWaitTime;

    [Header("Charge State")]
    public float chargeTime;
    public float chargeSpeed;

    [Header("Attack State")]
    public float attackDetectDistance;
    public float damageAmount;
    public Vector2 knockbackAngle;
    public float knockbackForce;

    [Header("Dodge State")]
    public Vector2 dodgeAngle;
    public float dodgeForce;
    public float dodgeDetectDistance;
    public float dodgeCooldown;

}
