﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public abstract class Audio : MonoBehaviour
{
    protected AudioSource AudioSource;

    private void Awake()
    {
        AudioSource = GetComponent<AudioSource>();
    }

    protected void PlayOneShot(AudioClip clip, int chance = 100, float volumeScale = 1f)
    {
        AudioSource.pitch = Random.Range(0.9f, 1.1f);

        if (Random.Range(0,100) < chance)
            AudioSource.PlayOneShot(clip, volumeScale);
    }

    protected void PlayOneShot(AudioClip clip, float delay, int chance = 100, float volumeScale = 1f)
    {
        StartCoroutine(PlayingOneShot(clip,delay,chance, volumeScale));
    }

    private IEnumerator PlayingOneShot(AudioClip clip, float delay, int chance = 100, float volumeScale = 1f)
    {
        yield return new WaitForSeconds(delay);

        PlayOneShot(clip, chance, volumeScale);
    }
}
