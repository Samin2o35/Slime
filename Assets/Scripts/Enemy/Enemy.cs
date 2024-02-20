using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    #region Variables
    
    [Header("Enemy States")]
    public EnemyBaseState currentState;

    public EnemyPatrolState patrolState;
    public EnemyDetectedPlayerState playerDetectedState;
    public EnemyChargeState chargeState;
    public EnemyAttackState attackState;

    [Header("Enemy Essentials")]
    public Rigidbody2D enemyRb;
    public Transform ledgeDetector;
    public Animator enemyAnim;
    public GameObject alert;
    public LayerMask groundLayer, obstacleLayer, playerLayer, damageableLayer;
    public EnemyStats enemyStats;

    [Header("Enemy Variables")]
    public int facingDirection = 1;
    public float stateTime;
    #endregion

    #region Unity Callbacks
    private void Awake()
    {
        patrolState = new EnemyPatrolState(this, "patrol");
        playerDetectedState = new EnemyDetectedPlayerState(this, "playerDetected");
        chargeState = new EnemyChargeState(this, "charge");
        attackState = new EnemyAttackState(this, "attack");

        currentState = patrolState;
        currentState.Enter();
    }

    private void Update()
    {
        currentState.LogicUpdate();
    }

    private void FixedUpdate()
    {
        currentState.PhysicsUpdate();
    }
    #endregion

    #region Enemy Checks
    public bool CheckForTerrain()
    {
        RaycastHit2D hitGround = Physics2D.Raycast(ledgeDetector.position, Vector2.down, 
            enemyStats.groundDistance, groundLayer);

        RaycastHit2D hitObstacle = Physics2D.Raycast(ledgeDetector.position, facingDirection == 1?
            Vector2.right : Vector2.left, enemyStats.obstacleDistance, obstacleLayer);

        if (hitGround.collider == null || hitObstacle.collider == true)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool CheckForPlayer()
    {
        RaycastHit2D hitPlayer = Physics2D.Raycast(ledgeDetector.position, facingDirection == 1? 
            Vector2.right : Vector2.left ,enemyStats.playerDetectDistance, playerLayer);

        if (hitPlayer.collider == true)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool CheckForAttackTarget()
    {
        RaycastHit2D hitAttackTarget = Physics2D.Raycast(ledgeDetector.position, facingDirection == 1 ?
            Vector2.right : Vector2.left, enemyStats.attackDetectDistance, damageableLayer);

        if (hitAttackTarget.collider == true)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    #endregion

    public void SwitchState(EnemyBaseState newState)
    {
        currentState.Exit();
        currentState = newState;
        currentState.Enter();
        stateTime = Time.time;
    }

    public void AnimationFinishedTrigger()
    {
        currentState.AnimationFinishedTrigger();
    }

    public void AnimationAttackTrigger()
    {
        currentState.AnimationAttackTrigger();
    }

    #region Debugging Region
    private void OnDrawGizmos()
    {
        // ground check
        Gizmos.color = Color.red;
        Gizmos.DrawRay(ledgeDetector.position, Vector2.down * enemyStats.groundDistance);

        // player check
        Gizmos.color = Color.green;
        Gizmos.DrawRay(ledgeDetector.position, (facingDirection == 1 ? Vector2.right : Vector2.left)
            * enemyStats.playerDetectDistance);

        // obstacle check
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(ledgeDetector.position, (facingDirection == 1? Vector2.right : Vector2.left)
            * enemyStats.obstacleDistance);

        // attack check
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(ledgeDetector.position, (facingDirection == 1 ? Vector2.right : Vector2.left)
            * enemyStats.attackDetectDistance);
    }
    #endregion
}
