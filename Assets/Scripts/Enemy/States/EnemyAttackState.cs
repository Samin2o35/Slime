using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackState : EnemyBaseState
{
    public EnemyAttackState(Enemy enemy, string animationName) :
        base(enemy, animationName)
    {

    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public override void AnimationAttackTrigger()
    {
        base.AnimationAttackTrigger();

        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(enemy.ledgeDetector.position,
            enemy.enemyStats.attackDetectDistance, enemy.damageableLayer);

        foreach (Collider2D hitCollider in hitColliders)
        {
            // check if objects hit have IDamageable script attached
            IDamageable damageable = hitCollider.GetComponent<IDamageable>();

            if (damageable != null)
            {
                hitCollider.GetComponent<Rigidbody2D>().velocity = new Vector2
                    (enemy.enemyStats.knockbackAngle.x * enemy.facingDirection,
                    enemy.enemyStats.knockbackAngle.y) * enemy.enemyStats.knockbackForce;

                damageable.Damage(enemy.enemyStats.damageAmount);
            }
        }
    }

    public override void AnimationFinishedTrigger()
    {
        base.AnimationFinishedTrigger();

        enemy.SwitchState(enemy.patrolState);
    }
}
