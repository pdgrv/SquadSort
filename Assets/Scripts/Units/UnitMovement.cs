using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class UnitMovement : MonoBehaviour
{
    private Animator _animator;
    private NavMeshAgent _agent;

    public event UnityAction ArrivedDestination;

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
        if (_agent.remainingDistance > 0.005f)
        {
            IsMooving = true;
        }
        else if (IsMooving)
        {
            IsMooving = false;
            ArrivedDestination?.Invoke();
        }

        _animator.SetBool("Run", IsMooving);
    }

    public void Move(Vector3 targetPoint)
    {
        _agent.SetDestination(targetPoint);
    }
}
