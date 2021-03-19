using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitEffects : MonoBehaviour
{
    [SerializeField] private ParticleSystem _attackFX;
    [SerializeField] private int _attackFXChance = 50;
    [SerializeField] private List<ParticleSystem> _applyDamageFX;
    [SerializeField] private int _applyDamageFXChance = 30;
    [SerializeField] private List<ParticleSystem> _dieFX;
    [SerializeField] private ParticleSystem _completedUnitFX;

    public void Attack()
    {
        if (_attackFX == null)
            return;

        if (Random.Range(0, 100) < _applyDamageFXChance)
        {
            _attackFX.Play();
        }
    }

    public void ApplyDamage()
    {
        if (Random.Range(0, 100) < _applyDamageFXChance)
        {
            _applyDamageFX[Random.Range(0, _applyDamageFX.Count)].Play();
        }
    }

    public void Die()
    {
        _dieFX[Random.Range(0, _dieFX.Count)].Play();
    }

    public void CompleteUnit()
    {
        _completedUnitFX.Play();
    }
}
