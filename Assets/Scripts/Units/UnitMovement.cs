using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//надо бы объединить все передвижение и повороты юнитов сюда как-то
public class UnitMovement : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 3f;
    [SerializeField] private float _rotationSpeed = 3f;
    [SerializeField] private float _stoppingDistance = 1.5f;

    private Animator _animator;

    private NavMeshAgent _agent;

    private Vector3 _targetPosition;

    public bool IsMooving { get; private set; } = false;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
    }
    private void OnDisable()
    {
        _agent.enabled = false;
    }

    private void FixedUpdate()
    {
        if (_agent.remainingDistance > 0.02f)
        {
            IsMooving = true;
        }
        else if (IsMooving)
        {
            IsMooving = false;

        }

        _animator.SetBool("Run", IsMooving);
    }

    public void Move(Vector3 targetPoint)
    {
        _agent.SetDestination(targetPoint);
    }

    private void LookDirection(Vector3 direction)
    {
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, _rotationSpeed * Time.deltaTime);
    }

    private void LookDirection(Vector3 currentPos, Vector3 targetPos)
    {
        Vector3 direction = targetPos - currentPos;
        LookDirection(direction);
    }
}
