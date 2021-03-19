using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Zombie : CombatUnit
{
    [SerializeField] private bool _isBoss = false;

    public event UnityAction<Zombie> ZombieDied;

    private ZombieBrain _brain;

    public bool IsBoss => _isBoss;

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

    public void Hide()
    {
        StartCoroutine(HideJob());
    }
    private IEnumerator HideJob() // не чисто
    {
        yield return new WaitForSeconds(3f);

        while (transform.position.y > -5)
        {
            transform.Translate(Vector3.down * Time.deltaTime);
            yield return new WaitForFixedUpdate();
        }

        gameObject.SetActive(false);
    }

    private void FindNewTarget()
    {
        CurrentTarget = _brain.GetRandomUnit();
    }

    protected override void Die(bool isBoss = false)
    {
        base.Die(_isBoss);

        ZombieDied?.Invoke(this);
    }
}
