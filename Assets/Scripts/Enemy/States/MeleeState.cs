using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeState : EnemyBaseState
{
    public MeleeState(Enemy enemy, string animationName) : base(enemy, animationName)
    {

    }

    public override void Enter()
    {
        base.Enter();

        Collider2D[] targets = Physics2D.OverlapCircleAll(enemy.enemyPos.position, enemy.stats.meleeAttackDistance, enemy.whatIsPlayer);
        
        foreach(Collider2D target in targets) 
        {
            IEnemyDamageable eDamageable = target.GetComponent<IEnemyDamageable>();

            if(eDamageable != null)
            {
                eDamageable.EnemyDamage(enemy.stats.eDamageAmount);
            }
            enemy.SwitchState(enemy.patrolState);
        }
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
}
