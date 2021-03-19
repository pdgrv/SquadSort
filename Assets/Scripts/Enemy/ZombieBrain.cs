using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Events;

public class ZombieBrain : MonoBehaviour
{
    [SerializeField] private List<Zombie> _zombies;

    [SerializeField] private List<Squad> _aliveSquads = new List<Squad>();
    [SerializeField] private List<CombatUnit> _frontUnits = new List<CombatUnit>();
    [SerializeField] private int _deadZombiesFillrate;

    public event UnityAction AllZombieKilled;

    private void Awake()
    {
        _zombies = FindObjectsOfType<Zombie>().ToList();
    }

    private void OnEnable()
    {
        for (int i = 0; i < _zombies.Count; i++)
        {
            _zombies[i].ZombieDied += OnZombieDied;
        }
    }

    private void OnDisable()
    {
        for (int i = 0; i < _zombies.Count; i++)
        {
            _zombies[i].ZombieDied -= OnZombieDied;
        }
    }

    public void StartInvasion(List<Squad> squads)
    {
        _aliveSquads = squads;

        _frontUnits.AddRange(_aliveSquads[0].CombatUnits);
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
                _frontUnits.AddRange(_aliveSquads[0].CombatUnits);
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

    private void OnZombieDied(Zombie zombie)
    {
        _zombies.Remove(zombie);

        if (_zombies.Count <= 0)
            AllZombieKilled?.Invoke();

        if (!zombie.IsBoss && Random.Range(0,100) > _deadZombiesFillrate)
        {
            zombie.Hide();
        }
    }
}
