using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrolState : EnemyBaseState
{
    public EnemyPatrolState(Enemy enemy, string animationName) : 
        base (enemy, animationName)
    {
        
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(enemy.CheckForPlayer())
        {
            enemy.SwitchState(enemy.playerDetectedState);
        }

        if (enemy.CheckForTerrain())
        {
            enemy.Rotate();
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        if (enemy.facingDirection == 1)
        {
            enemy.enemyRb.velocity = new Vector2(enemy.enemyStats.enemyMoveSpeed, enemy.enemyRb.velocity.y);
        }
        else
        {
            enemy.enemyRb.velocity = new Vector2(-enemy.enemyStats.enemyMoveSpeed, enemy.enemyRb.velocity.y);
        }
    }
}
