using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "EnemyStatsSO")]
public class EnemyStats : ScriptableObject
{
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
}
