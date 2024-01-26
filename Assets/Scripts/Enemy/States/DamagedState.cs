using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagedState : EnemyBaseState
{
    public float KBForce;
    public Vector2 KBAngle;
    
    public DamagedState(Enemy enemy, string animationName) : base(enemy, animationName)
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
    }

    private void ApplyKnockback()
    {
        enemy.enemyRb.velocity = KBAngle * KBForce;
    }
}
