using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class BackgroundMusic : MonoBehaviour
{
    [SerializeField] private AudioSource _sortingMusic;
    [SerializeField] private AudioSource _battleMusic;
    [SerializeField] private float _transitionTime = 1.5f;
    [SerializeField] private AudioMixerGroup _musicMixer;
    [SerializeField] private float mixerBattleVolume;
    [SerializeField] private float mixerSortingVolume;

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
        ChangeMusic(_sortingMusic, mixerSortingVolume);
    }

    public void StartBattle()
    {
        ChangeMusic(_battleMusic, mixerBattleVolume);
    }

    private void ChangeMusic(AudioSource targetSource, float mixerTargetVolume)
    {
        if (_crossfadeJob != null)
            StopCoroutine(_crossfadeJob);

        _crossfadeJob = StartCoroutine(Crossfade(targetSource, mixerTargetVolume, _transitionTime));
    }

    private IEnumerator Crossfade(AudioSource targetSource, float mixerTargetVolume, float duration)
    {
        float currentTime = 0;
        float startVolume = _currentSource.volume;
        targetSource.Play();

        if (_musicMixer.audioMixer.GetFloat("MusicVolume", out float mixerVolume))
            Debug.Log("MusVol - " + mixerVolume);

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            _currentSource.volume = Mathf.Lerp(startVolume, 0f, currentTime / duration);
            targetSource.volume = Mathf.Lerp(0, startVolume, currentTime / duration);

            _musicMixer.audioMixer.SetFloat("MusicVolume", Mathf.LerpUnclamped(mixerVolume, mixerTargetVolume, currentTime / duration));

            yield return null;
        }

        _currentSource.Stop();
        _currentSource = targetSource;
        yield break;
    }
}
