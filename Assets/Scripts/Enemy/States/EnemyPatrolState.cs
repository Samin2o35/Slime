using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.RuleTile.TilingRuleOutput;

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

    public override void Exit()
    {
        base.Exit();
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
            Rotate();
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        if (enemy.facingRight)
        {
            enemy.enemyRb.velocity = new Vector2(enemy.enemyMoveSpeed, enemy.enemyRb.velocity.y);
        }
        else
        {
            enemy.enemyRb.velocity = new Vector2(-enemy.enemyMoveSpeed, enemy.enemyRb.velocity.y);
        }
    }

    private void Rotate()
    {
        enemy.transform.Rotate(0, 180, 0);
        enemy.facingRight = !enemy.facingRight;
    }
}
