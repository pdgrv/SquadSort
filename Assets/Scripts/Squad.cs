using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Squad : MonoBehaviour
{
    [SerializeField] private int _fullSquadCount = 4;
    [SerializeField] private List<Unit> _units;
    [SerializeField] private SortingContainer _currentContainer;

    public event UnityAction SquadFulled;

    public UnitType UnitsType => _units[0].Type;

    public int UnitsCount => _units.Count;

    private void OnValidate()
    {
        CheckSquadUnits();
    }

    private void CheckSquadUnits()
    {
        if (_units != null)
        {
            foreach (Unit unit in _units)
            {
                if (unit.Type != UnitsType) { 
                    Debug.Log("Разные юниты в скваде!");
                    _units.Remove(unit);
                }
            }
        }
    }

    public void PushSquadFrom(Squad departingSquad)
    {
        _units.AddRange(departingSquad.GetUnits());

        RenderSquad();

        if (UnitsCount >= _fullSquadCount)
            SquadFulled?.Invoke();
    }

    public void ClearSquad()
    {
        _units.Clear();
        gameObject.SetActive(false);
    }

    public List<Unit> GetUnits()
    {
        return _units;
    }

    private void RenderSquad()
    {
        for (int i = 0; i < _units.Count; i++)
        {
            _units[i].transform.parent = transform;
            _units[i].transform.localPosition = new Vector3(0, 0, i);
        }
    }
}
