using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class PatrolState : EnemyBaseState
{
    public PatrolState(Enemy enemy, string animationName) : base(enemy, animationName)
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
            Rotate();
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        if (enemy.isFacingRight)
        {
            enemy.enemyRb.velocity = new Vector2(enemy.enemySpeed, enemy.enemyRb.velocity.y);
        }
        else
        {
            enemy.enemyRb.velocity = new Vector2(-enemy.enemySpeed, enemy.enemyRb.velocity.y);
        }
    }

    private void Rotate()
    {
        enemy.transform.Rotate(0, 180, 0);
        enemy.isFacingRight = !enemy.isFacingRight;
    }
}
