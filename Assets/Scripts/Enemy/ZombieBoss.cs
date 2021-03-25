using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieBoss : Zombie
{
    protected override void Die()
    {
        Animator.SetInteger("DieID", 2);
        Animator.SetTrigger("Die");

        UnitEffects.Die();

        if (TryGetComponent(out Collider collider))
            collider.enabled = false;

        enabled = false;

        RaiseZombieDiedEvent();
    }

    public override void Hide()
    {
        return;
    }
}
