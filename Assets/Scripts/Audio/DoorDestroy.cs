using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorDestroy : Audio
{
    [SerializeField] private AudioClip _smallHit;
    [SerializeField] private AudioClip _bigHit;
    [SerializeField] private AudioClip _fall;

    public void SmallHit()
    {
        PlayOneShot(_smallHit);
    }

    public void BigHit()
    {
        PlayOneShot(_bigHit);
    }

    public void Fall()
    {
        AudioSource.volume = 0.8f;
        PlayOneShot(_fall);
    }
}
