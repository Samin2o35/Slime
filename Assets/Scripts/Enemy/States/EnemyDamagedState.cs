using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamagedState : EnemyBaseState
{
    // how far enemy knocked back after getting hit
    public float kBForce;
    public Vector2 kBAngle;

    // how long enemy stunned after getting hit
    public float stunTime;
    public bool isStunned;
    
    public EnemyDamagedState(Enemy enemy, string animationName) :
        base(enemy, animationName)
    {

    }

    public override void Enter()
    {
        base.Enter();

        ApplyKnockback();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(Time.time > enemy.stateTime + stunTime) 
        {
            isStunned = false;

            int attackDirection = enemy.enemyRb.velocity.x > 0 ? -1 : 1;

            if (enemy.facingDirection != attackDirection) 
            {
                enemy.Rotate();
            }
            enemy.SwitchState(enemy.chargeState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
    public override void AnimationAttackTrigger()
    {
        base.AnimationAttackTrigger();
    }

    public override void AnimationFinishedTrigger()
    {
        base.AnimationFinishedTrigger();

        enemy.enemyRb.velocity = new Vector2(enemy.enemyRb.velocity.x, 
            -enemy.enemyRb.velocity.y);
        isStunned = true;
    }

    private void ApplyKnockback()
    {
        enemy.enemyRb.velocity = kBAngle * kBForce;
    }
}
