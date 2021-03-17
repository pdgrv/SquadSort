using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieBrain : MonoBehaviour
{
    [SerializeField] private Zombie[] _zombies;

    private List<CombatUnit> _units = new List<CombatUnit>();

    private void Awake()
    {
        _zombies = FindObjectsOfType<Zombie>();
    }

    public void StartInvasion(List<Squad> squads)
    {
        foreach (Squad squad in squads)
        {
            _units.AddRange(squad.GetCombatUnits());
        }

        foreach (Zombie zombie in _zombies)
        {
            zombie.SetTarget(_units[Random.Range(0, _units.Count)]);
        }
    }

    private void UpdateUnits()
    {
        List<CombatUnit> unitsAlive = new List<CombatUnit>();

        foreach (CombatUnit unit in _units)
        {
            if (unit.IsAlive)
                unitsAlive.Add(unit);
        }

        _units = unitsAlive;
    }

    public CombatUnit GetRandomUnit()
    {
        UpdateUnits();

        return _units.Count > 0 ? _units[Random.Range(0, _units.Count)] : null;
    }
}
