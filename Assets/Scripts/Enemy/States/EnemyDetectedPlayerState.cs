using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetectedPlayerState : EnemyBaseState
{
    public EnemyDetectedPlayerState(Enemy enemy, string animationName) : 
        base(enemy, animationName)
    {

    }

    public override void Enter()
    {
        base.Enter();

        enemy.enemyRb.velocity = Vector2.zero;
        enemy.alert.SetActive(true);
    }

    public override void Exit()
    {
        base.Exit();

        enemy.alert.SetActive(false);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (enemy.CheckIfShouldDodge())
        {
            enemy.SwitchState(enemy.dodgeState);
        }
        else if (!enemy.CheckForPlayer())
        {
            enemy.SwitchState(enemy.patrolState);
        }
        else
        {
            if(Time.time >= enemy.stateTime + enemy.enemyStats.playerDetectedWaitTime)
            {
                enemy.SwitchState(enemy.chargeState);
            }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
