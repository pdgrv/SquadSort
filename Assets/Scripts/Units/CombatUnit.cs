using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Animator))]
public abstract class CombatUnit : MonoBehaviour
{
    [SerializeField] private int _health = 1;
    [SerializeField] private int _damage = 1;
    [SerializeField] protected float AttackDistance = 1.5f;
    [SerializeField] private float _attackDelay = 1f;
    [SerializeField] protected float MoveSpeed = 4;

    protected Animator Animator;
    protected CombatUnit CurrentTarget;

    protected float LastAttackTimer = 0f;

    public bool IsAlive => _health > 0;

    private void Awake()
    {
        Animator = GetComponent<Animator>();
    }

    private void LateUpdate()
    {
        if (!IsAlive)
        {
            if (TryGetComponent(out Collider collider))
                collider.enabled = false;
            enabled = false;
        }
    }

    public virtual void Attack(CombatUnit target)
    {
        transform.LookAt(target.transform.position);

        if (LastAttackTimer < 0f)
        {
            target.ApplyDamage(_damage);
            Animator.SetTrigger("Attack");

            LastAttackTimer = _attackDelay;
        }
    }

    public virtual void ApplyDamage(int damage)
    {
        if (_health - damage > 0)
        {
            Animator.SetTrigger("ApplyDamage");
            _health -= damage;
        }
        else
        {
            _health = 0;
            Die();
        }
    }

    public virtual void Move(CombatUnit target)
    {
        transform.LookAt(target.transform.position);

        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, Time.deltaTime * MoveSpeed);
    }

    public void SetTarget(CombatUnit target)
    {
        CurrentTarget = target;
    }

    public void CelebrateWictory()
    {
        Animator.SetTrigger("Celebrate");
    }

    protected virtual void Die()
    {
        Animator.SetTrigger("Die");
    }
}