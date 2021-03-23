using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherUnit : InfantryUnit
{
    [SerializeField] private Arrow _arrow;

    private const float _arrowSpeedMultiplier = 1.1f;

    protected override void Attack(CombatUnit target)
    {
        ShootArrow();

        base.Attack(target);
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