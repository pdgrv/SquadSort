using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Unit : MonoBehaviour
{
    [SerializeField] private UnitType _type;
    [SerializeField] private float _speed = 5f;
    [SerializeField] private UnitEffects _unitEffects;

    private UnitMovement _movement;

    private Animator _animator;
    private CombatUnit _combatUnit;

    private Vector3 _lookAtPlayer;
    private Coroutine _movementJob;
    private Coroutine _lookAtJob;

    public bool IsMove { get; private set; } = false;

    public Coroutine MovementJob => _movementJob;
    public CombatUnit CombatUnit => _combatUnit;
    public UnitType Type => _type;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _combatUnit = GetComponent<CombatUnit>();
        _lookAtPlayer = Camera.main.transform.position;
        _lookAtPlayer.y = transform.position.y;

        _movement = GetComponent<UnitMovement>();
    }

    private void OnEnable()
    {
        _movement.ArrivedDestination += OnArrivedDestination;
    }

    private void OnDisable()
    {
        _movement.ArrivedDestination -= OnArrivedDestination;
    }


    //public void Move(Vector3 newPosition)
    //{
    //    if (_movementJob != null)
    //        StopCoroutine(_movementJob);

    //    _movementJob = StartCoroutine(Movement(newPosition));
    //}

    public void Move(Vector3 newPosition)
    {
        IsMove = true;
        _movement.Move(newPosition);

        if (_lookAtJob != null)
            StopCoroutine(_lookAtJob);
    }

    public void Select()
    {
        LookAt(_lookAtPlayer);
        _animator.SetBool("LookUp", true);

        _unitEffects.Select();
    }

    public void Unselect()
    {
        LookAt(transform.position + Vector3.forward);
        _animator.SetBool("LookUp", false);

        _unitEffects.UnselectUnit();
    }

    public void EnterCombatStance()
    {
        _animator.SetBool("CombatStance", true);

        _unitEffects.Complete();
    }

    private void OnArrivedDestination()
    {
        IsMove = false;
        LookAt(transform.position + Vector3.forward);
    }

    private void LookAt(Vector3 target)
    {
        if (_lookAtJob != null)
            StopCoroutine(_lookAtJob);

        _lookAtJob = StartCoroutine(LookAtJob(target));
    }

    private IEnumerator LookAtJob(Vector3 target)
    {
        Vector3 direction = target - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(direction);

        while (Quaternion.Angle(transform.rotation, lookRotation) > 1f)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, 7f * Time.deltaTime);
            yield return null;
        }

        transform.rotation = lookRotation;

        _lookAtJob = null;
    }
}

public enum UnitType
{
    Knight,
    Soldier,
    Archer
}