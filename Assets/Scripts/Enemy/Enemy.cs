using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    #region Variables
    public EnemyBaseState currentState; 

    public EnemyPatrolState patrolState;
    public EnemyDetectedPlayerState playerDetectedState;

    public Rigidbody2D enemyRb;
    public Transform ledgeDetector;
    public LayerMask groundLayer, obstacleLayer, playerLayer;

    public bool facingRight = true;

    public float groundDistance, obstacleDistance, playerDetectDistance;
    public float enemyMoveSpeed;
    public float detectionPauseTime;
    public GameObject alert;
    #endregion

    #region Unity Callbacks
    private void Awake()
    {
        patrolState = new EnemyPatrolState(this, "patrol");
        playerDetectedState = new EnemyDetectedPlayerState(this, "playerDetected");

        currentState = patrolState;
        currentState.Enter();
    }

    private void FixedUpdate()
    {
        currentState.PhysicsUpdate();
    }

    private void Update()
    {
        currentState.LogicUpdate();
    }
    #endregion

    #region Enemy Checks
    public bool CheckForTerrain()
    {
        RaycastHit2D hit = Physics2D.Raycast(ledgeDetector.position, Vector2.down, 
            groundDistance, groundLayer);

        RaycastHit2D hitObstacle = Physics2D.Raycast(ledgeDetector.position, facingRight?
            Vector2.right : Vector2.left, obstacleDistance, obstacleLayer);

        if (hit.collider == null || hitObstacle.collider == true)
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
        RaycastHit2D hitPlayer = Physics2D.Raycast(ledgeDetector.position, facingRight? 
            Vector2.right : Vector2.left ,playerDetectDistance, playerLayer);

        if (hitPlayer.collider == true)
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
    }

    #region Debugging Region
    private void OnDrawGizmos()
    {
        // ground check
        Gizmos.color = Color.red;
        Gizmos.DrawRay(ledgeDetector.position, Vector2.down * groundDistance);

        // player check
        Gizmos.color = Color.green;
        Gizmos.DrawRay(ledgeDetector.position, (facingRight ? Vector2.right : Vector2.left) 
            * playerDetectDistance);

        // obstacle check
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(ledgeDetector.position, (facingRight ? Vector2.right : Vector2.left)
            * obstacleDistance);
    }
    #endregion
}
