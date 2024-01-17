using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : EnemyBaseState
{
    public AttackState(Enemy enemy, string animationName) : base(enemy, animationName)
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

        //charge at player if detected, resume patrol if player not detected
        if (Time.time >= enemy.stateTime + enemy.dashTime)
        {
            if (enemy.CheckForPlayer())
            {
                enemy.SwitchState(enemy.playerDetectedState);
            }
            else
            {
                enemy.SwitchState(enemy.patrolState);
            }
        }
        else 
        {
            Dash();
        }
    }

    public void Dash()
    {
        enemy.enemyRb.velocity = new Vector2(enemy.dashSpeed * enemy.isFacingDirection, enemy.enemyRb.velocity.y);
    }
}
