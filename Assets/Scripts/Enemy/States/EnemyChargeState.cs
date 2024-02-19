using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChargeState : EnemyBaseState
{
    public EnemyChargeState(Enemy enemy, string animationName) :
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

        if(Time.time >= enemy.stateTime + enemy.enemyStats.chargeTime)
        {
            if(enemy.CheckForPlayer())
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
            Charge();
        }
    }

    private void Charge() 
    {
        enemy.enemyRb.velocity = new Vector2(enemy.enemyStats.chargeSpeed * enemy.facingDirection,
            enemy.enemyRb.velocity.y);
    }
}
