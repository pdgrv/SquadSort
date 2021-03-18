using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Zombie : CombatUnit
{
    public event UnityAction<Zombie> ZombieDied;

    private ZombieBrain _brain;

    private int DieID = 0;
    private const int DieMaxID = 2;

    private void Start()
    {
        _brain = GetComponentInParent<ZombieBrain>();
    }

    private void Update()
    {
        LastAttackTimer -= Time.deltaTime;

        if (CurrentTarget == null)
            return;

        if (CurrentTarget.IsAlive)
        {
            if (Vector3.Distance(transform.position, CurrentTarget.transform.position) > AttackDistance)
            {
                Animator.SetBool("Run", true);
                Animator.ResetTrigger("Attack");
                Move(CurrentTarget);
            }
            else
            {
                Animator.SetBool("Run", false);
                Attack(CurrentTarget);
            }
        }
        else
        {
            Animator.SetBool("Run", false);
            FindNewTarget();
        }
    }

    private void FindNewTarget()
    {
        CurrentTarget = _brain.GetRandomUnit();
    }

    protected override void Die()
    {
        base.Die();

        DieID = Random.Range(0, DieMaxID + 1);

        Animator.SetInteger("DieID", DieID);

        ZombieDied?.Invoke(this);
    }
}
