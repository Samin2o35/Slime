using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Unity.VisualScripting;

public class Enemy : MonoBehaviour, IDamageable
{
    private Rigidbody2D enemyRb;
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

    [Header("Patrol")]
    [SerializeField] Transform groundCheckPoint;
    [SerializeField] Transform wallCheckPoint;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private float circleRadius;
    [SerializeField] private float moveSpeed;

    private float moveDirection = 1f;
    private bool isFacingRight = true;
    private bool isCheckingForGround;
    private bool isCheckingForWall;

    private void Start()
    {
        currentHealth = maxHealth;
        enemyAnim = GetComponent<Animator>();
        enemyRb = GetComponent<Rigidbody2D>();
        healthBar = GetComponentInChildren<HealthBar>();
        impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    private void FixedUpdate()
    {
        isCheckingForGround = Physics2D.OverlapCircle(groundCheckPoint.position, circleRadius, whatIsGround);
        isCheckingForWall = Physics2D.OverlapCircle(wallCheckPoint.position, circleRadius, whatIsGround);
        Patrol();
    }

    private void Patrol()
    {
        if(!isCheckingForGround || isCheckingForWall)
        {
            if(isFacingRight)
            {
                Flip();
            }
            else if(!isFacingRight)
            {
                Flip();
            }
        }
        enemyAnim.SetBool("isRunning", true);
        enemyRb.velocity = new Vector2(moveSpeed * moveDirection, enemyRb.velocity.y);
    }

    private void Flip()
    {
        moveDirection *= -1f;
        isFacingRight = !isFacingRight;
        transform.Rotate(0, 180, 0);
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

    private void SpawnDamageParticles(Vector2 attackDirection)
    {
        Quaternion spawnRotation = Quaternion.FromToRotation(Vector2.right, attackDirection);
        damageParticlesInstance = Instantiate(damageParticles, transform.position, spawnRotation);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(groundCheckPoint.position, circleRadius);
        Gizmos.DrawWireSphere(wallCheckPoint.position, circleRadius);
    }
}