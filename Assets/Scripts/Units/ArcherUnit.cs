using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherUnit : CombatUnit
{
    [SerializeField] private Arrow _arrow;

    private const float _arrowSpeedMultiplier = 3f;

    private void Update()
    {
        LastAttackTimer -= Time.deltaTime;
    }

    private void FixedUpdate()
    {
        Collider[] enemies = Physics.OverlapSphere(transform.position, AttackDistance);

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
            ShootArrow();

            Attack(CurrentTarget);
        }
    }

    private void ShootArrow() 
    {
        DamageAnticipationTimer = Vector3.Distance(transform.position, CurrentTarget.transform.position) / _arrow.Speed * _arrowSpeedMultiplier;

        if (LastAttackTimer < 0f)
        {
            _arrow.Shoot(CurrentTarget);
        }
    }
}