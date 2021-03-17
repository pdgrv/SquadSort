using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : CombatUnit
{
    private ZombieBrain _brain;

    private void Start()
    {
        _brain = GetComponentInParent<ZombieBrain>();
    }

    private void Update()
    {
        LastAttackTimer += Time.deltaTime;

        if (CurrentTarget == null)
            return;

        if (CurrentTarget.IsAlive)
        {
            if (Vector3.Distance(transform.position, CurrentTarget.transform.position) > AttackDistance)
            {
                Animator.SetBool("Run", true);
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
}
