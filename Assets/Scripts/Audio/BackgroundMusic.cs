using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    [SerializeField] private AudioSource _sortingMusic;
    [SerializeField] private AudioSource _battleMusic;
    [SerializeField] private float _transitionTime = 1.5f;

    private Coroutine _crossfadeJob;
    private AudioSource _currentSource;

    private void Awake()
    {
        BackgroundMusic[] loadedObjects = FindObjectsOfType<BackgroundMusic>();
        if (loadedObjects.Length > 1)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        _currentSource = _sortingMusic;
    }

    public void SortingMusic()
    {
        ChangeMusic(_sortingMusic);
    }

    public void StartBattle()
    {
        ChangeMusic(_battleMusic);
    }

    private void ChangeMusic(AudioSource targetSource)
    {
        if (_crossfadeJob != null)
            StopCoroutine(_crossfadeJob);

        _crossfadeJob = StartCoroutine(Crossfade(targetSource, _transitionTime));
    }

    private IEnumerator Crossfade(AudioSource targetSource, float duration)
    {
        float currentTime = 0;
        float startVolume = _currentSource.volume;
        targetSource.Play();

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            _currentSource.volume = Mathf.Lerp(startVolume, 0f, currentTime / duration);
            targetSource.volume = Mathf.Lerp(0, startVolume, currentTime / duration);
            yield return null;
        }

        _currentSource.Stop();
        _currentSource = targetSource;
        yield break;
    }
}
