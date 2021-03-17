﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Squad : MonoBehaviour
{
    [SerializeField] private int _fullSquadCount = 4;
    [SerializeField] private List<Unit> _units;

    public event UnityAction SquadFulled;

    public UnitType UnitsType => _units[0].Type;

    public int UnitsCount => _units.Count;

    private void OnValidate()
    {
        CheckEqualsUnitsType();
    }

    public void Combine(Squad fromSquad)
    {
        for (int i = 0; i < fromSquad.UnitsCount; i++)
        {
            _units.Add(fromSquad._units[i]);
        }
    }

    public void CheckCompleteSquad()
    {
        if (UnitsCount >= _fullSquadCount)
            CompleteSquad();
    }

    public void SelectSquad()
    {
        for (int i = 0; i < _units.Count; i++)
        {
            _units[i].Select();
        }
    }

    public void UnselectSquad()
    {
        for (int i = 0; i < _units.Count; i++)
        {
            _units[i].Unselect();
        }
    }

    public void MoveSquad(Squad targetSquad, Vector3 targetSquadPosition, Vector2 orientation)
    {
        UnselectSquad();

        for (int i = 0; i < _units.Count; i++)
        {
            _units[i].transform.parent = targetSquad.transform;
            _units[i].Move(new Vector3(orientation.x, 0, orientation.y) * (i) + targetSquadPosition);
        }
    }

    public void ClearSquad()
    {
        _units.Clear();
        gameObject.SetActive(false);
    }

    public List<CombatUnit> GetCombatUnits()
    {
        List<CombatUnit> combatUnits = new List<CombatUnit>();

        foreach (Unit unit in _units)
            combatUnits.Add(unit.CombatUnit);

        return combatUnits;
    }


    private void CompleteSquad()
    {
        for (int i = 0; i < _units.Count; i++)
        {
            _units[i].EnterCombatStance();
        }

        SquadFulled?.Invoke();
    }

    private void CheckEqualsUnitsType()
    {
        if (_units != null)
        {
            foreach (Unit unit in _units)
            {
                if (unit.Type != UnitsType)
                {
                    Debug.Log("Разные юниты в скваде!");
                    _units.Remove(unit);
                }
            }
        }
    }
}
