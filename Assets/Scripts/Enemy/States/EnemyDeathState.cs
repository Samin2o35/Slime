using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyDeathState : EnemyBaseState
{
    public EnemyDeathState(Enemy enemy, string animationName) :
        base(enemy, animationName)
    {

    }

    public override void Enter()
    {
        base.Enter();

        DeathParticles();
        DropItems();
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
    private void DeathParticles()
    {
        enemy.Instantiate(enemy.enemyStats.deathParticle, enemy.dropForce, enemy.torque);

        foreach (var debris in enemy.enemyStats.deathDebris)
        {
            enemy.Instantiate(debris, 1, 0);
        }
        enemy.gameObject.SetActive(false);
    }

    private void DropItems()
    {
        foreach(var itemDrops in enemy.itemDrops)
        {
            enemy.Instantiate(itemDrops, enemy.dropForce, enemy.torque);
        }
    }
}
