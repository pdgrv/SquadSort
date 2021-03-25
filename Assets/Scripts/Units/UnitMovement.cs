using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//надо бы объединить все передвижение и повороты юнитов сюда как-то
public class UnitMovement : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 3f;
    [SerializeField] private float _rotationSpeed = 3f;
    [SerializeField] private float _stoppingDistance = 1.5f;

    private Vector3 _targetPosition;

    public bool IsMooving { get; private set; }

    private void Update()
    {
        if (_targetPosition == null)
            return;
        else if (Vector3.Distance(transform.position, _targetPosition) > _stoppingDistance)
            Move();
        else
            IsMooving = false;
    }

    private void Move()
    {
        IsMooving = true;

        LookDirection(transform.position, _targetPosition);

        transform.position = Vector3.MoveTowards(transform.position, _targetPosition, Time.deltaTime * _moveSpeed);
    }

    private void LookDirection(Vector3 direction)
    {
        if (direction.magnitude < 0.05f)
            return;

        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, _rotationSpeed * Time.deltaTime);
    }

    private void LookDirection(Vector3 currentPos, Vector3 targetPos)
    {
        Vector3 direction = targetPos - currentPos;
        LookDirection(direction);
    }
}
