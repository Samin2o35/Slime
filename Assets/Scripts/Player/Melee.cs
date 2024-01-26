using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : MonoBehaviour
{
    #region Variable Region

    [SerializeField] private Transform AttackTransform;
    [SerializeField] private LayerMask attackableLayer;
    [SerializeField] private float attackRange;
    [SerializeField] private float damageAmount;
    [SerializeField] private float timeBetweenAttack;
    private RaycastHit2D[] hits;
    private Animator anim;
    private List<IDamageable> targets = new List<IDamageable>();
    private float attackTimeCounter;
    public bool ShouldBeDamaging { get; private set; } = false;

    #endregion

    private void Start()
    {
        anim = GetComponent<Animator>();

        //Allow attack to be performed immediately
        attackTimeCounter = timeBetweenAttack;
    }

    private void Update()
    {
        if (UserInput.instance.controls.Attack.Melee.WasPressedThisFrame() && attackTimeCounter >= timeBetweenAttack)
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
            hits = Physics2D.CircleCastAll(AttackTransform.position, attackRange, transform.right, 0f, attackableLayer);
            for (int i = 0; i < hits.Length; i++)
            {
                IDamageable iDamageable = hits[i].collider.gameObject.GetComponent<IDamageable>();
                if (iDamageable != null && iDamageable.HasTakenDamage == false)
                {
                    //apply damage to hit targets
                    iDamageable.PDamage(damageAmount, transform.right);
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
        Gizmos.DrawWireSphere(AttackTransform.position, attackRange);
    }

    #endregion
}
