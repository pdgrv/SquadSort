using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField] private int _totalSquads;
    [SerializeField] private List<Squad> _squads;

    private int _totalFulledSquads = 0;


    private void Awake()
    {
        _squads.AddRange(FindObjectsOfType<Squad>());
    }

    private void OnEnable()
    {
        foreach (Squad squad in _squads)
        {
            squad.SquadFulled += OnSquadFulled;
        }
    }
    private void OnDisable()
    {
        foreach (Squad squad in _squads)
        {
            squad.SquadFulled -= OnSquadFulled;
        }
    }

    private void OnSquadFulled()
    {
        _totalFulledSquads++;
        if (_totalFulledSquads >= _totalSquads)
            SortingComplete();
    }

    private void SortingComplete()
    {
        Debug.Log("Сортировка закончена!");
    }

}
