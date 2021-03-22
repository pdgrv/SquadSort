using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Unit : MonoBehaviour
{
    [SerializeField] private UnitType _type;
    [SerializeField] private float _speed = 5f;
    [SerializeField] private UnitEffects _unitEffects;

    private Animator _animator;
    private CombatUnit _combatUnit;

    private Vector3 _lookAtPlayer;
    private Coroutine _movementJob;
    private Coroutine _lookAtJob;

    public CombatUnit CombatUnit => _combatUnit;
    public UnitType Type => _type;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _combatUnit = GetComponent<CombatUnit>();
        _lookAtPlayer = Camera.main.transform.position;
        _lookAtPlayer.y = transform.position.y;
    }

    public void Move(Vector3 newPosition)
    {
        if (_movementJob != null)
            StopCoroutine(_movementJob);

        _movementJob = StartCoroutine(Movement(newPosition));
    }

    public void Select()
    {
        //transform.LookAt(_lookAtPlayer);
        LookAt(_lookAtPlayer);
        _animator.SetBool("LookUp", true);

        _unitEffects.SelectUnit();
    }

    public void Unselect()
    {
        //transform.LookAt(transform.position + Vector3.forward);
        LookAt(transform.position + Vector3.forward);
        _animator.SetBool("LookUp", false);


        _unitEffects.UnselectUnit();
    }

    public void EnterCombatStance()
    {
        _animator.SetBool("CombatStance", true);

        _unitEffects.CompleteUnit();
    }

    private IEnumerator Movement(Vector3 newPosition)
    {
        _animator.SetBool("Run", true);

        //transform.LookAt(new Vector3(newPosition.x, transform.position.y, newPosition.z));
        LookAt(new Vector3(newPosition.x, transform.position.y, newPosition.z));

        while (Vector3.Distance(transform.position, newPosition) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, newPosition, Time.deltaTime * _speed);

            yield return null;
        }

        transform.position = newPosition;
        _animator.SetBool("Run", false);

        //transform.LookAt(transform.position + Vector3.forward);
        LookAt(transform.position + Vector3.forward);

        _movementJob = null;
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