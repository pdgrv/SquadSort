using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Animator))]
public abstract class CombatUnit : MonoBehaviour
{
    [SerializeField] private int _health = 1;
    [SerializeField] private int _damage = 1;
    [SerializeField] protected float DamageAnticipationTimer;
    [SerializeField] protected float AttackDistance = 1.5f;
    [SerializeField] private float _attackDelay = 1f;
    [SerializeField] protected float MoveSpeed = 4;
    [SerializeField] protected UnitEffects UnitEffects;
    [SerializeField] private float _rotationSpeed = 5f;

    [SerializeField] private int _attackMaxID = 0;
    [SerializeField] private int _dieMaxID = 0;

    protected Animator Animator;
    protected CombatUnit CurrentTarget;

    //protected UnitMovement _movement;

    protected float LastAttackTimer = 0f;

    public bool IsAlive => _health > 0;

    private void Awake()
    {
        Animator = GetComponent<Animator>();
        //_movement = GetComponent<UnitMovement>();
    }

    public void SetTarget(CombatUnit target)
    {
        CurrentTarget = target;
    }

    public void CelebrateWictory()
    {
        Animator.ResetTrigger("Attack");
        Animator.SetTrigger("Celebrate");
    }

    public void ApplyDamage(int damage, float timer)
    {
        StartCoroutine(ApplyDamageTimer(damage, timer));
    }

    protected virtual void Attack(CombatUnit target)
    {
        LookDirection(target.transform.position - transform.position);

        if (LastAttackTimer < 0f)
        {
            target.ApplyDamage(_damage, DamageAnticipationTimer);
            LastAttackTimer = _attackDelay;

            int AttackID = Random.Range(0, _attackMaxID + 1);
            Animator.SetInteger("AttackID", AttackID);
            Animator.SetTrigger("Attack");

            UnitEffects.Attack();

            AttackSound();
        }
    }

    protected virtual void Move(CombatUnit target) //используется сейчас только в зомби.
    {
        LookDirection(target.transform.position - transform.position);
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, Time.deltaTime * MoveSpeed);

        //_movement.Move(target.transform.position);
    }


    protected virtual void Die() //что-то тут не чисто
    {
        int DieID = Random.Range(0, _dieMaxID + 1);

        Animator.SetInteger("DieID", DieID);
        Animator.SetTrigger("Die");

        UnitEffects.Die();
        DiedSound();

        enabled = false;
        //_movement.enabled = false;
    }

    private IEnumerator ApplyDamageTimer(int damage, float timer)
    {
        yield return new WaitForSeconds(timer);

        if (_health > damage)
        {
            _health -= damage;

            Animator.SetTrigger("ApplyDamage");
            UnitEffects.ApplyDamage();
            ApplyDamageSound();
        }
        else if (_health > 0)
        {
            _health = 0;
            Die();
        }
    }

    protected abstract void AttackSound();

    protected abstract void ApplyDamageSound();

    protected abstract void DiedSound();

    private void LookDirection(Vector3 direction)
    {
        if (direction.magnitude < 0.05f)
            return;

        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, _rotationSpeed * Time.deltaTime);
    }

}