using System.Collections.Generic;
using UnityEngine;

public class SquadsContainer : MonoBehaviour
{
    [SerializeField] private int _maxUnits = 4;
    [SerializeField] private List<Squad> _currentSquads;
    [SerializeField] private float _unitZStep = 1f;

    public bool IsFree => _currentSquads.Count == 0 ? true : false;
    private Squad LastSquad => _currentSquads[_currentSquads.Count - 1];

    private int TotalUnits
    {
        get
        {
            int value = 0;
            foreach (Squad squad in _currentSquads)
                value += squad.UnitsCount;
            return value;
        }
    }

    private void OnValidate()
    {
    }

    public bool TryInteract(SquadsContainer fromContainer) // не нравится название и что возвращает bool isNewSelect
    {
        Squad fromSquad = fromContainer.LastSquad;

        if (IsFree)
        {
            MoveSquadUnits(fromSquad, fromSquad);
            AddSquad(fromSquad);

            fromContainer.RemoveSquad(fromSquad);

            return false;
        }
        else
        {
            Squad targetSquad = LastSquad;

            if (IsEnoughCapacity(fromSquad)) // переместить внутрь
            {
                if (fromSquad.UnitsType == targetSquad.UnitsType)
                {
                    MoveSquadUnits(fromSquad, targetSquad);
                    targetSquad.Combine(fromSquad);

                    fromContainer.RemoveSquad(fromSquad);
                    fromSquad.ClearSquad();

                    Debug.Log("отряд из " + fromContainer.name + " совмещаем с " + name);
                    return false;
                }
                else
                {
                    Debug.Log("отряды контейнеров не сопадают, выбрать новый контейнер");
                    return true;
                }
            }
            else
            {
                Debug.Log(name + " не может вместить столько юнитов");
                FocusBad();

                return false;
            }
        }
    }

    private void MoveSquadUnits(Squad fromSquad, Squad targetSquad)
    {
        Vector3 targetSquadPosition = transform.position - new Vector3(0, 0, 1.5f) + new Vector3(0, 0, _unitZStep) * TotalUnits;

        fromSquad.MoveSquad(targetSquad, targetSquadPosition);
    }

    private void AddSquad(Squad squad)
    {
        _currentSquads.Add(squad);
    }

    private void RemoveSquad(Squad squad)
    {
        _currentSquads.Remove(squad);
    }

    private bool IsEnoughCapacity(Squad fromSquad)
    {
        return TotalUnits + fromSquad.UnitsCount <= _maxUnits;
    }

    public void FocusOn()
    {

    }

    public void FocusOff()
    {

    }

    public void FocusBad()
    {

    }
}
