﻿using System.Collections;
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
    [SerializeField] public UnitEffects UnitEffects;//для complete unit  
    [SerializeField] private float _rotationSpeed = 10f;

    [SerializeField] private int _attackMaxID = 0;
    [SerializeField] private int _dieMaxID = 0;

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
        //transform.LookAt(target.transform.position);
        LookDirection(target.transform.position-transform.position);

        if (LastAttackTimer < 0f)
        {
            target.ApplyDamage(_damage, DamageAnticipationTimer);
            LastAttackTimer = _attackDelay;

            int AttackID = Random.Range(0, _attackMaxID + 1);
            Animator.SetInteger("AttackID", AttackID);
            Animator.SetTrigger("Attack");

            UnitEffects.Attack();
        }
    }

    public void ApplyDamage(int damage, float timer)
    {
        StartCoroutine(ApplyDamageTimer(damage, timer));
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
        Animator.ResetTrigger("Attack");
        Animator.SetTrigger("Celebrate");
    }

    protected virtual void Die(bool isBoss = false) //что-то тут не чисто
    {
        int DieID = Random.Range(0, _dieMaxID + 1);

        if (isBoss)
            Animator.SetInteger("DieID", _dieMaxID);
        else
            Animator.SetInteger("DieID", DieID);

        Animator.SetTrigger("Die");

        UnitEffects.Die();
    }

    private IEnumerator ApplyDamageTimer(int damage, float timer)
    {
        yield return new WaitForSeconds(timer);

        if (_health > damage)
        {
            _health -= damage;

            Animator.SetTrigger("ApplyDamage");
            UnitEffects.ApplyDamage();
        }
        else if (_health >= 1 )  //чтото тут не чисто
        {
            _health = 0;
            Die();
        }
    }

    private void LookDirection(Vector3 direction)
    {
        if (direction.magnitude < 0.05f)
            return;

        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, _rotationSpeed * Time.deltaTime);
    }

    //void SmoothLook(Vector3 newDirection)
    //{
    //    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(newDirection), _rotationSpeed * Time.deltaTime);
    //}
}