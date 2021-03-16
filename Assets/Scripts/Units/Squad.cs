using System.Collections;
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
        _units.AddRange(fromSquad._units);

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

    public void MoveSquad(Squad targetSquad, Vector3 targetSquadPosition)
    {
        UnselectSquad();

        for (int i = 0; i < _units.Count; i++)
        {
            _units[i].transform.parent = targetSquad.transform;
            _units[i].Move(new Vector3(0, 0, i) + targetSquadPosition);
        }
    }

    public void ClearSquad()
    {
        _units.Clear();
        gameObject.SetActive(false);
    }

    private void CompleteSquad()
    {
        SquadFulled?.Invoke();

        for (int i = 0; i < _units.Count; i++)
        {
            _units[i].EnterCombatStance();
        }
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
