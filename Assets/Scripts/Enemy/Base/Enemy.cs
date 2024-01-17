using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Unity.VisualScripting;

public class Enemy : MonoBehaviour, IDamageable
{
    public EnemyBaseState currentState;
    
    public PatrolState patrolState;
    public PlayerDetectedState playerDetectedState;

    #region Variable region

    public Rigidbody2D enemyRb;
    private Animator enemyAnim;

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

    [Header("Checkers")]
    public Transform ledgeDetector;
    public Transform enemyPos;
    public GameObject alert;

    [Header("Check Environment")]
    public LayerMask whatIsGround;
    public LayerMask whatIsObstacle;
    public LayerMask whatIsPlayer;

    [Header("Check Distance")]
    public float rayCastDistance;
    public float obstacleDistance;
    public float playerDetectDistance;

    [Header("Enemy Variables")]
    public float enemySpeed;
    public float playerDetectPauseTime;

    [Header("Booleans")]
    public bool isFacingRight = true;

    #endregion

    private void Awake()
    {
        patrolState = new PatrolState(this, "patrol");
        playerDetectedState = new PlayerDetectedState(this, "playerDetected");

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
        RaycastHit2D groundHit = Physics2D.Raycast(ledgeDetector.position, Vector2.down, rayCastDistance, whatIsGround);
        RaycastHit2D obstacleHit = Physics2D.Raycast(ledgeDetector.position, Vector2.right, obstacleDistance, whatIsObstacle);

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
        RaycastHit2D playerDetectHitLeft = Physics2D.Raycast(enemyPos.position, Vector2.left, playerDetectDistance, whatIsPlayer);
        RaycastHit2D playerDetectHitRight = Physics2D.Raycast(enemyPos.position, Vector2.right, playerDetectDistance, whatIsPlayer);

        if (playerDetectHitLeft.collider == true || playerDetectHitRight.collider == true)
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
        Gizmos.DrawWireSphere(enemyPos.position, playerDetectDistance);

        Gizmos.color = Color.red;
        Gizmos.DrawRay(ledgeDetector.position, Vector2.down * rayCastDistance);

        Gizmos.color = Color.blue;
        Gizmos.DrawRay(ledgeDetector.position, (isFacingRight ? Vector2.right : Vector2.left) * obstacleDistance);
    }

    #endregion
}
