using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetectedState : EnemyBaseState
{
    public PlayerDetectedState(Enemy enemy, string animationName) : base (enemy, animationName)
    {
        
    }

    public override void Enter()
    {
        base.Enter();

        enemy.enemyRb.velocity = Vector2.zero;
        enemy.stats.alert.SetActive(true);
    }

    public override void Exit()
    {
        enemy.stats.alert.SetActive(false);
    }
    

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(!enemy.CheckForPlayer())
        {
            enemy.SwitchState(enemy.patrolState);
        }
        else
        {
            if(Time.time >= enemy.stateTime + enemy.stats.playerDetectedWaitTime)
            {
                enemy.SwitchState(enemy.attackState);
            }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
