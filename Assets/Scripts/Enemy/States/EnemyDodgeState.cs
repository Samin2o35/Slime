using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class EnemyDodgeState : EnemyBaseState
{
    private float endDodgeForce = 5f;
    private float endDodgeTime = 0.2f;
    private bool dodgeStarted;
    private bool dodgeEnded;
    public EnemyDodgeState(Enemy enemy, string animationName) :
        base(enemy, animationName)
    {

    }

    public override void AnimationAttackTrigger()
    {
        base.AnimationAttackTrigger();
    }

    public override void AnimationFinishedTrigger()
    {
        base.AnimationFinishedTrigger();
    }

    public override void Enter()
    {
        base.Enter();

        dodgeStarted = false;
        dodgeEnded = false;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (Time.time > enemy.stateTime + 0.5f && !dodgeStarted)
        {
            StartDodge();
        }

        if(Time.time > enemy.stateTime + enemy.enemyStats.dodgeCooldown) 
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
        else if(Time.time > enemy.stateTime + endDodgeTime && !dodgeEnded)
        {
            EndDodge();
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    private void StartDodge()
    {
        enemy.enemyRb.velocity = new Vector2(enemy.enemyStats.dodgeAngle.x
            * -enemy.facingDirection, enemy.enemyStats.dodgeAngle.y)
            * enemy.enemyStats.dodgeForce;
        dodgeStarted = true;
    }

    private void EndDodge()
    {
        enemy.enemyRb.velocity = new Vector2(.1f * -enemy.facingDirection, -1f) 
            * endDodgeForce;
        dodgeEnded = true;
    }
}
