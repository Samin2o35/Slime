using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    #region Variables

    [Header("Enemy States")]
    public EnemyBaseState currentState;

    public EnemyPatrolState patrolState;
    public EnemyDetectedPlayerState playerDetectedState;
    public EnemyChargeState chargeState;
    public EnemyAttackState attackState;
    public EnemyDamagedState damagedState;
    public EnemyDeathState deathState;
    public EnemyDodgeState dodgeState;

    [Header("Enemy Essentials")]
    public Rigidbody2D enemyRb;
    public Transform ledgeDetector;
    public Animator enemyAnim;
    public Animator hitParticle;
    public GameObject alert;
    public LayerMask groundLayer, obstacleLayer, playerLayer, damageableLayer;
    public EnemyStats enemyStats;
    public float currentHealth;

    [Header("Enemy Variables")]
    public int facingDirection = 1;
    public float stateTime;

    [Header("Item Drop Variables")]
    public GameObject[] itemDrops;
    public float dropForce;
    public float torque;
    #endregion

    #region Unity Callbacks
    private void Start()
    {
        currentHealth = enemyStats.maxHealth;
    }

    private void Awake()
    {
        patrolState = new EnemyPatrolState(this, "patrol");
        playerDetectedState = new EnemyDetectedPlayerState(this, "playerDetected");
        chargeState = new EnemyChargeState(this, "charge");
        attackState = new EnemyAttackState(this, "attack");
        damagedState = new EnemyDamagedState(this, "damaged");
        deathState = new EnemyDeathState(this, "death");
        dodgeState = new EnemyDodgeState(this, "dodge");

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

        RaycastHit2D hitObstacle = Physics2D.Raycast(ledgeDetector.position, facingDirection == 1 ?
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
        RaycastHit2D hitPlayer = Physics2D.Raycast(ledgeDetector.position, facingDirection == 1 ?
            Vector2.right : Vector2.left, enemyStats.playerDetectDistance, playerLayer);

        if (hitPlayer.collider == true)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool CheckIfShouldDodge()
    {
        RaycastHit2D hitPlayer = Physics2D.Raycast(ledgeDetector.position, facingDirection == 1 ?
            Vector2.right : Vector2.left, enemyStats.dodgeDetectDistance, playerLayer);
        
        // trigger dodge only if player attacking
        bool aggroPlayer = facingDirection > 0 && Input.GetAxis("Horizontal") < 0
            || facingDirection < 0 && Input.GetAxis("Horizontal") > 0;
        if(hitPlayer && aggroPlayer)
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

    public void Rotate()
    {
        transform.Rotate(0, 180, 0);
        facingDirection = -facingDirection;
    }

    public void Instantiate(GameObject prefab, float force, float torque)
    {
        Rigidbody2D itemRb = Instantiate(prefab, transform.position, 
            Quaternion.identity).GetComponent<Rigidbody2D>();

        Vector2 dropVeloctiy = new Vector2(Random.Range(-0.5f, 0.5f), 1) * dropForce;
        itemRb.AddForce(dropVeloctiy, ForceMode2D.Impulse);
        itemRb.AddTorque(torque, ForceMode2D.Impulse);
    }

    #region Animation Triggers Region
    public void AnimationFinishedTrigger()
    {
        currentState.AnimationFinishedTrigger();
    }

    public void AnimationAttackTrigger()
    {
        currentState.AnimationAttackTrigger();
    }
    #endregion

    #region Damage Region
    public void Damage(float damageAmount)
    {
        
    }

    // enemy takes damage
    public void Damage(float damageAmount, float kBForce, Vector2 kBAngle)
    {
        hitParticle.Play("Enemy - HitParticle");
        damagedState.kBForce = kBForce;
        damagedState.kBAngle = kBAngle;
        currentHealth -= damageAmount;

        if(currentHealth < 0)
        {
            SwitchState(deathState);
        }
        else
        {
            SwitchState(damagedState);
        }
    }
    #endregion

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
