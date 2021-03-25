using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Zombie : CombatUnit
{
    public event UnityAction<Zombie> ZombieDied;

    private ZombieBrain _brain;

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

    public virtual void Hide()
    {
        StartCoroutine(Hiding());
    }

    private IEnumerator Hiding()
    {
        yield return new WaitForSeconds(3f);

        while (transform.position.y > -5)
        {
            transform.Translate(Vector3.down * Time.deltaTime * 0.5f);
            yield return new WaitForFixedUpdate();
        }

        gameObject.SetActive(false);
    }

    private void FindNewTarget()
    {
        CurrentTarget = _brain.GetRandomUnit();
    }

    protected override void Die()
    {
        base.Die();

        if (TryGetComponent(out Collider collider))
            collider.enabled = false;

        RaiseZombieDiedEvent();
    }

    protected void RaiseZombieDiedEvent()
    {
        ZombieDied?.Invoke(this);
    }

    protected override void AttackSound()
    {
        ZombieAudio.Instance.Attack();
    }

    protected override void ApplyDamageSound()
    {
    }

    protected override void DiedSound()
    {
        ZombieAudio.Instance.Die();
    }
}
