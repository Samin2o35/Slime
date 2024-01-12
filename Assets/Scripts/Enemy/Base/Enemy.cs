using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Unity.VisualScripting;

public class Enemy : MonoBehaviour, IDamageable, IEnemyMoveable
{
    private CinemachineImpulseSource impulseSource;
    private ParticleSystem damageParticlesInstance;
    private HealthBar healthBar;

    [SerializeField] private ScreenShakeProfile profile;
    [SerializeField] private float maxHealth;
    [SerializeField] private ParticleSystem damageParticles;
    private float currentHealth;
    public bool HasTakenDamage { get; set; }
    public Rigidbody2D rb { get; set; }
    public bool isFacingRight { get; set; } = true;

    // state machines
    public EnemyCurrentState stateMachine { get; set; }
    public EnemyIdleState idleState { get; set; }
    public EnemyChaseState chaseState { get; set; }
    public EnemyAttackState attackState { get; set; }

    private void Awake()
    {
        stateMachine = new EnemyCurrentState();

        idleState = new EnemyIdleState(this, stateMachine);
        chaseState = new EnemyChaseState(this,stateMachine);
        attackState = new EnemyAttackState(this, stateMachine);

    }
    private void Start()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        healthBar = GetComponentInChildren<HealthBar>();
        impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    private void Update()
    {
        stateMachine.currentEnemyState.FrameUpdate();
    }

    private void FixedUpdate()
    {
        stateMachine.currentEnemyState.PhysicsUpdate();
    }

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

    private void SpawnDamageParticles(Vector2 attackDirection)
    {
        Quaternion spawnRotation = Quaternion.FromToRotation(Vector2.right, attackDirection);
        damageParticlesInstance = Instantiate(damageParticles, transform.position, spawnRotation);
    }

    public void MoveEnemy(Vector2 velocity)
    {
        rb.velocity = velocity;
        CheckFacingDirection(velocity);
    }

    public void CheckFacingDirection(Vector2 velocity)
    {
        if (isFacingRight && velocity.x < 0f)
        {
            Vector3 rotator = new Vector3(transform.rotation.x, 180f, transform.rotation.z);
            transform.rotation = Quaternion.Euler(rotator);
            isFacingRight = !isFacingRight;
        }
        else if (isFacingRight && velocity.x > 0f)
        {
            Vector3 rotator = new Vector3(transform.rotation.x, 0f, transform.rotation.z);
            transform.rotation = Quaternion.Euler(rotator);
            isFacingRight = !isFacingRight;
        }
    }

    private void AnimationTriggerEvent(AnimationTriggerType trigger)
    {
        stateMachine.currentEnemyState.AnimationTriggerEvent(trigger);
    }

    public enum AnimationTriggerType
    {
        EnemyDamaged,
        PlayFootstepSound
    }
}
