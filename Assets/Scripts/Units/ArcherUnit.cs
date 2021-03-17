using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherUnit : CombatUnit
{
    private void Update()
    {
        LastAttackTimer -= Time.deltaTime;
    }

    private void FixedUpdate()
    {
        Collider[] enemies = Physics.OverlapSphere(transform.position, AttackDistance);

        if (CurrentTarget == null)
        {
            CurrentTarget = enemies[Random.Range(0, enemies.Length)].GetComponent<CombatUnit>();
        }
        else if (!CurrentTarget.IsAlive)
        {
            CurrentTarget = null;
        }
        else
        {
            Attack(CurrentTarget);
        }
    }
}