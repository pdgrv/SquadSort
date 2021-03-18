using System.Collections.Generic;
using UnityEngine;

public class SquadsContainer : MonoBehaviour
{
    [SerializeField] private int _maxUnits = 4;
    [SerializeField] private List<Squad> _currentSquads;
    [SerializeField] private float _unitZStep = 1f;

    private Animation _animation;

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

    private void Awake()
    {
        _animation = GetComponent<Animation>();
    }

    private void OnValidate()
    {
    }

    public bool TryMoveSquad(SquadsContainer fromContainer) // не нравится что возвращает bool !isNewSelect
    {
        Squad fromSquad = fromContainer.LastSquad;

        if (IsFree)
        {
            MoveSquadUnits(fromSquad, fromSquad);
            AddSquad(fromSquad);

            fromContainer.RemoveSquad(fromSquad);
        }
        else
        {
            Squad targetSquad = LastSquad;

            if (fromSquad.UnitsType == targetSquad.UnitsType)
            {
                if (IsEnoughCapacity(fromSquad))
                {
                    MoveSquadUnits(fromSquad, targetSquad);
                    targetSquad.Combine(fromSquad);

                    fromSquad.ClearSquad();
                    fromContainer.RemoveSquad(fromSquad);

                    targetSquad.CheckCompleteSquad();

                    Debug.Log("отряд из " + fromContainer.name + " совмещаем с " + name);
                }
                else
                {
                    Debug.Log(name + " не может вместить столько юнитов");
                    FocusBad();
                }
            }
            else
            {
                Debug.Log("отряды контейнеров не сопадают, выбрать новый контейнер");
                return false;
            }
        }

        return true;
    }

    private void MoveSquadUnits(Squad fromSquad, Squad targetSquad)
    {
        Vector3 targetSquadPosition = transform.position - new Vector3(0, 0, 1.5f) + new Vector3(0, 0, _unitZStep) * TotalUnits;

        fromSquad.MoveSquad(targetSquad, targetSquadPosition, new Vector2(0, 1));
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
        LastSquad.SelectSquad();
    }

    public void FocusOff()
    {
        if (!IsFree)
            LastSquad.UnselectSquad();
    }

    public void FocusBad()
    {
        _animation.Play();
    }
}
