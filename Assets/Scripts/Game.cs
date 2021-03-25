using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Game : MonoBehaviour
{
    [SerializeField] private int _totalSquads;
    [SerializeField] private List<Squad> _squads;
    [SerializeField] private ZombieBrain _zombieBrain;
    [SerializeField] private CameraController _cameraController;
    [SerializeField] private Animation _castleDoorsAnimation;

    private ReloadScene _debugReloadScene;
    private BackgroundMusic _backgroundMusic;

    private List<SquadsContainer> _containers = new List<SquadsContainer>();

    private int _totalFulledSquads = 0;

    private void Awake()
    {
        _squads.AddRange(FindObjectsOfType<Squad>());
        _containers.AddRange(FindObjectsOfType<SquadsContainer>());
        _backgroundMusic = FindObjectOfType<BackgroundMusic>();

        _debugReloadScene = GetComponent<ReloadScene>();
        _debugReloadScene.enabled = false;

        Time.timeScale = 0.5f;
    }

    private void OnEnable()
    {
        foreach (Squad squad in _squads)
        {
            squad.SquadFulled += OnSquadFulled;
        }

        _zombieBrain.AllZombieKilled += OnAllZombieKilled;
    }


    private void OnDisable()
    {
        foreach (Squad squad in _squads)
        {
            squad.SquadFulled -= OnSquadFulled;
        }

        _zombieBrain.AllZombieKilled -= OnAllZombieKilled;
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

        StartCoroutine(StartBattle());
    }

    private IEnumerator StartBattle()
    {
        _backgroundMusic.StartBattle();
        yield return new WaitForSeconds(2f);

        foreach (SquadsContainer container in _containers)
            container.gameObject.SetActive(false);

        List<Squad> completedSquads = new List<Squad>();

        foreach (Squad squad in _squads)
        {
            if (squad.gameObject.activeSelf)
                completedSquads.Add(squad);
        }

        yield return new WaitForSeconds(0.5f);

        BuildRanks(completedSquads);

        _cameraController.ActivateCombatMode();
        _castleDoorsAnimation.Play(); //тайминг в анимации

        _zombieBrain.StartInvasion(completedSquads);
    }

    private void BuildRanks(List<Squad> squads)
    {
        squads.Sort((x, y) => ((int)x.UnitsType).CompareTo((int)y.UnitsType));

        //float targetPosX = 6f / (squads[1].UnitsCount + 1) - 3f; //6f - примерная ширина камеры

        for (int i = 0; i < squads.Count; i++)
        {
            squads[i].MoveSquad(squads[i], new Vector3(-2.4f, 0, 4f - i * 1.6f), new Vector2(1.4f, 0f)); //magic
        }
    }

    private void CompleteBattle()
    {
        _debugReloadScene.enabled = true;

        UnitsAudio.Instance.Celebate();
        _backgroundMusic.SortingMusic();
    }

    private void OnAllZombieKilled()
    {
        CompleteBattle();

        foreach (Squad squad in _squads)
        {
            if (squad.enabled)
                squad.Celebrate();
        }
    }
}
