using System.Collections.Generic;
using UnityEngine;

public class SortingContainer : MonoBehaviour
{
    [SerializeField] private int _maxUnits = 4;
    [SerializeField] private List<Squad> _currentSquads;
    [SerializeField] private float _unitZStep = 1;

    public bool IsFree => _currentSquads.Count > 0 ? false : true;

    private void OnValidate()
    {
    }

    public void Interact(ref SortingContainer fromContainer)
    {
        if (fromContainer.Equals(this))
            return; //отменить выбор
        else
        {
            Squad fromSquad = fromContainer.TryGetLastSquad();

            if (fromContainer.IsFree)
            {
                fromContainer = null;
                return;
            }

            if (IsFree)
            {
                SimpleSquadMove(fromSquad);

                fromContainer.RemoveSquad(fromSquad);

                fromContainer = null;
                return;
            }

            Squad targetSquad = TryGetLastSquad(); //если вернет Null?

            if (TotalUnitCount() + fromSquad.UnitsCount > _maxUnits)//тут часто ошибка
            {
                Debug.Log(name + " не может вместить столько юнитов");
                fromContainer = null;
                return;
            }

            if (SquadCombiner.TryCombineSquads(fromSquad, targetSquad))
            {
                fromContainer.RemoveSquad(fromSquad);  //это плохо - один контейнер решает за другой
                Debug.Log(name + "совмещаем с " + fromContainer.name);
            }
            else
            {
                fromContainer = this;
                Debug.Log("Выбран контейнер " + name);
            }

        }

    }

    private void SimpleSquadMove(Squad squad)
    {
        squad.transform.localPosition = transform.position - new Vector3(0, 0, 1.5f);

        AddSquad(squad);
    }

    public Squad TryGetLastSquad()
    {
        if (_currentSquads.Count > 0)
            return _currentSquads[_currentSquads.Count - 1];
        else
            return null;
    }

    public void AddSquad(Squad squad)
    {
        _currentSquads.Add(squad);
    }

    public void RemoveSquad(Squad squad)
    {
        _currentSquads.Remove(squad);
    }

    public int TotalUnitCount()
    {
        int unitCount = 0;

        foreach (Squad squad in _currentSquads)
            unitCount += squad.UnitsCount;

        return unitCount;
    }
}
