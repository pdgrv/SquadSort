using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private GameObject _arrow;
    [SerializeField] private ParticleSystem _projectileFX;
    [SerializeField] private ParticleSystem _impactFX;
    [SerializeField] private Transform _startingPoint;

    private bool _isFly = false;
    private CombatUnit _currentTarget;
    private bool _firstShoot = true;

    private Vector3 _targetHitOffset;

    public float Speed => _speed;

    private void Awake()
    {
        _arrow.SetActive(false);

        _targetHitOffset = new Vector3(Random.Range(-0.1f, 0.1f), Random.Range(0.8f, 1.1f), 0f);
    }

    private void Update()
    {
        if (_currentTarget != null && _isFly)
        {
            transform.position = Vector3.MoveTowards(transform.position, _currentTarget.transform.position + _targetHitOffset
                , Time.deltaTime * _speed);

            transform.LookAt(_currentTarget.transform.position + _targetHitOffset);

            if (Vector3.Distance(transform.position, _currentTarget.transform.position + _targetHitOffset) < 0.1f)
            {
                _isFly = false;
                _impactFX.Play();

                _arrow.SetActive(false);
            }
        }
    }

    public void Shoot(CombatUnit target)
    {
        _currentTarget = target;

        StartCoroutine(ShootJob());
    }

    private IEnumerator ShootJob()
    {
        transform.parent = null;
        yield return new WaitForSeconds(0.2f);

        transform.position = _startingPoint.position;
        transform.rotation = _startingPoint.rotation;
        _arrow.SetActive(true);
        _isFly = true;
    }
}
