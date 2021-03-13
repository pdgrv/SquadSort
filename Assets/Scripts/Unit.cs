using System.Collections;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] private UnitType _type;
    [SerializeField] private float _speed = 5f;

    private Coroutine _movementJob;

    public UnitType Type => _type;

    public void Move(Vector3 newPosition)
    {
        if (_movementJob != null)
            StopCoroutine(_movementJob);
            
       _movementJob =  StartCoroutine(Movement(newPosition));
    }

    private IEnumerator Movement(Vector3 newPosition)
    {
        while (Vector3.Distance(transform.position, newPosition)> 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, newPosition, Time.deltaTime * _speed);

            yield return new WaitForFixedUpdate();
        }

        transform.position = newPosition;
        _movementJob = null;
    }
}

public enum UnitType
{
    Soldier,
    Archer,
    Knight
}