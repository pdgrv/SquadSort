using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieBrain : MonoBehaviour
{
    [SerializeField] private Zombie[] _zombies;

    [SerializeField]private List<Squad> _aliveSquads = new List<Squad>();
    [SerializeField]private List<CombatUnit> _frontUnits = new List<CombatUnit>();

    private void Awake()
    {
        _zombies = FindObjectsOfType<Zombie>();
    }

    public void StartInvasion(List<Squad> squads)
    {
        _aliveSquads = squads;

        _frontUnits.AddRange(_aliveSquads[0].GetCombatUnits());
        _aliveSquads.RemoveAt(0);

        foreach (Zombie zombie in _zombies)
        {
            zombie.SetTarget(_frontUnits[Random.Range(0, _frontUnits.Count)]);
        }
    }

    private void UpdateUnits()
    {
        List<CombatUnit> unitsAlive = new List<CombatUnit>();

        foreach (CombatUnit unit in _frontUnits)
        {
            if (unit.IsAlive)
                unitsAlive.Add(unit);
        }

        if (unitsAlive.Count <= 0)
        {
            if (_aliveSquads.Count > 0)
            {
                _frontUnits.AddRange(_aliveSquads[0].GetCombatUnits());
                _aliveSquads.RemoveAt(0);
            }
        }
        else
        {
            _frontUnits = unitsAlive;
        }
    }

    public CombatUnit GetRandomUnit()
    {
        UpdateUnits();

        return _frontUnits.Count > 0 ? _frontUnits[Random.Range(0, _frontUnits.Count)] : null;
    }
}
