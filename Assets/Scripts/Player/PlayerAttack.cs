using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    #region Variable Region

    private Animator anim;
    public PlayerStats pStats;

    [Header("Check Layers")]
    [SerializeField] private Transform AttackTransform;
    [SerializeField] private LayerMask attackableLayer;

    [Header("Attack Enemies")]
    private List<IDamageable> targets = new List<IDamageable>();
    private RaycastHit2D[] hits;
    public bool ShouldBeDamaging { get; private set; } = false;
    private float attackTimeCounter;

    #endregion

    private void Start()
    {
        anim = GetComponent<Animator>();

        //Allow attack to be performed immediately
        attackTimeCounter = pStats.timeBetweenAttack;
    }

    private void Update()
    {
        if (UserInput.instance.controls.Attack.Melee.WasPressedThisFrame() && attackTimeCounter >= pStats.timeBetweenAttack)
        {
            //reset attack counter
            attackTimeCounter = 0f;
            
            //Attack();
            anim.SetTrigger("Melee");
        }

        attackTimeCounter += Time.deltaTime;
    }

    #region Damage Enemy
    public IEnumerator DamageWhileAttackActive()
    {
        ShouldBeDamaging = true;
        while(ShouldBeDamaging)
        {
            hits = Physics2D.CircleCastAll(AttackTransform.position, pStats.attackRange, transform.right, 0f, attackableLayer);
            for (int i = 0; i < hits.Length; i++)
            {
                IDamageable iDamageable = hits[i].collider.gameObject.GetComponent<IDamageable>();
                if (iDamageable != null && iDamageable.HasTakenDamage == false)
                {
                    //apply damage to hit targets
                    iDamageable.PDamage(pStats.damageAmount, transform.right, pStats.KBForce, pStats.KBAngle);
                    targets.Add(iDamageable);
                }
            }
            yield return null;
        }
        ReturnTargetsToDamageable();
    }

    private void ReturnTargetsToDamageable()
    {
        foreach(IDamageable hitTargets in targets)
        {
            hitTargets.HasTakenDamage = false;
        }

        targets.Clear();
    }

    #endregion

    #region AnimationTriggers

    public void ShouldBeDamagingToTrue()
    {
        ShouldBeDamaging = true;
    }

    public void ShouldBeDamagingToFalse()
    {
        ShouldBeDamaging= false;
    }

    #endregion

    #region Debug

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(AttackTransform.position, pStats.attackRange);
    }

    #endregion

    /*private void OnTriggerEnter2D(Collider2D collision)
    {
        IDamageable damageable = collision.GetComponent<IDamageable>();

        if(damageable != null)
        {
            
            damageable.PDamage(pStats.damageAmount, transform.right, pStats.KBForce, pStats.KBAngle);
        }
    }*/
}
