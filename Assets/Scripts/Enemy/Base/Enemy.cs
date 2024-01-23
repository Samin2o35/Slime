using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Unity.VisualScripting;

public class Enemy : MonoBehaviour, IDamageable
{
    #region Variable region

    public Rigidbody2D enemyRb;
    public Animator enemyAnim;

    [Header("State Machines")]
    public EnemyBaseState currentState;

    public PatrolState patrolState;
    public PlayerDetectedState playerDetectedState;
    public AttackState attackState;
    public MeleeState meleeState;

    [Header("ScreenShake")]
    [SerializeField] private ScreenShakeProfile profile;
    private CinemachineImpulseSource impulseSource;

    [Header("Health")]
    [SerializeField] private float maxHealth;
    private HealthBar healthBar;
    private float currentHealth;
    public bool HasTakenDamage { get; set; }

    [Header("Particles")]
    private ParticleSystem damageParticlesInstance;
    [SerializeField] private ParticleSystem damageParticles;

    [Header("Check Environment")]
    public LayerMask whatIsGround;
    public LayerMask whatIsObstacle;
    public LayerMask whatIsPlayer;

    [Header("Checkers")]
    public Transform ledgeDetector;
    public Transform enemyPos;
    public GameObject alert;

    [Header("Booleans")]
    public int isFacingDirection = 1; //prevents writing multiple if statements

    public float stateTime; //keep track of time when we enter new state
    public EnemyStats stats;

    #endregion

    private void Awake()
    {
        patrolState = new PatrolState(this, "isPatrolling");
        playerDetectedState = new PlayerDetectedState(this, "isPlayerDetected");
        attackState = new AttackState(this, "isDashing");
        meleeState = new MeleeState(this, "isAttacking");

        currentState = patrolState;
        currentState.Enter();
    }

    private void Start()
    {
        currentHealth = maxHealth;
        enemyAnim = GetComponent<Animator>();
        enemyRb = GetComponent<Rigidbody2D>();
        healthBar = GetComponentInChildren<HealthBar>();
        impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    private void Update()
    {
        currentState.LogicUpdate();
    }

    private void FixedUpdate()
    {
        currentState.PhysicsUpdate();
    }

    #region Player Detection and Patrol region
    public bool CheckForTerrain()
    {
        RaycastHit2D groundHit = Physics2D.Raycast(ledgeDetector.position, Vector2.down, stats.groundDistance, whatIsGround);
        RaycastHit2D obstacleHit = Physics2D.Raycast(ledgeDetector.position, Vector2.right, stats.obstacleDistance, whatIsObstacle);

        if (groundHit.collider == null || obstacleHit.collider == true)
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
        RaycastHit2D playerDetectHitLeft = Physics2D.Raycast(enemyPos.position, Vector2.left, stats.playerDetectDistance, whatIsPlayer);
        RaycastHit2D playerDetectHitRight = Physics2D.Raycast(enemyPos.position, Vector2.right, stats.playerDetectDistance, whatIsPlayer);

        if (playerDetectHitLeft.collider == true || playerDetectHitRight.collider == true)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool CheckForMeleeTarget()
    {
        RaycastHit2D hitMeleeTargetLeft = Physics2D.Raycast(enemyPos.position, Vector2.left, stats.meleeAttackDistance, whatIsPlayer);
        RaycastHit2D hitMeleeTargetRight = Physics2D.Raycast(enemyPos.position, Vector2.right, stats.meleeAttackDistance, whatIsPlayer);

        if (hitMeleeTargetLeft.collider == true || hitMeleeTargetRight.collider == true)
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

    #region Damage region
    public void Damage(float damageAmount, Vector2 attackDirection)
    {
        CameraShakeManager.instance.ScreenShakeFromProfile(profile, impulseSource);
        HasTakenDamage = true;
        currentHealth -= damageAmount;

        //spawn particles
        SpawnDamageParticles(attackDirection);

        //update health bar according to damage taken
        healthBar.UpdateHealthBar(maxHealth, currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(this.gameObject);
    }

    public void AnimationFinishedTrigger()
    {
        // tell current state to run finsh trigger
        currentState.AnimationFinishedTrigger();
    }

    public void AnimationAttackTrigger()
    {
        // tell current state to run attack trigger
        currentState.AnimationAttackTrigger();
    }

    #endregion

    #region Particle Region

    private void SpawnDamageParticles(Vector2 attackDirection)
    {
        Quaternion spawnRotation = Quaternion.FromToRotation(Vector2.right, attackDirection);
        damageParticlesInstance = Instantiate(damageParticles, transform.position, spawnRotation);
    }

    #endregion

    #region Debug region

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(enemyPos.position, stats.playerDetectDistance);

        Gizmos.color = Color.red;
        Gizmos.DrawRay(ledgeDetector.position, Vector2.down * stats.groundDistance);

        Gizmos.color = Color.blue;
        Gizmos.DrawRay(ledgeDetector.position, (isFacingDirection == 1 ? Vector2.right : Vector2.left) * stats.obstacleDistance);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(enemyPos.position, stats.meleeAttackDistance);
    }

    #endregion
}
