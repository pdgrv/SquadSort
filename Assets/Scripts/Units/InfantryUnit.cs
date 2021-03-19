using System.Collections;
using UnityEngine;

public class InfantryUnit : CombatUnit
{
    private void Update()
    {
        LastAttackTimer -= Time.deltaTime;
    }

    private void FixedUpdate()
    {
        Collider[] enemies = Physics.OverlapSphere(transform.position, AttackDistance); //дубляж кода с ArcheryUnit

        if (CurrentTarget == null)
        {
            if (enemies.Length > 0 && enemies[Random.Range(0, enemies.Length)].TryGetComponent(out CombatUnit currentTarget))
                CurrentTarget = currentTarget;
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
