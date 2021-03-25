using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorDestroy : Audio
{
    [SerializeField] private Animation _castleDoorsAnimation;
    [SerializeField] private AudioClip _smallHit;
    [SerializeField] private AudioClip _bigHit;
    [SerializeField] private AudioClip _fall;

    public void SmallHit()
    {
        PlayOneShot(_smallHit, volumeScale:0.9f);
    }

    public void BigHit()
    {
        PlayOneShot(_bigHit, volumeScale:0.85f);
    }

    public void Fall()
    {
        PlayOneShot(_fall, volumeScale: 0.7f);
        PlayOneShot(_fall, 0.01f, volumeScale:0.65f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out ZombieBoss boss))
        {
            _castleDoorsAnimation.Play();
            GetComponent<Collider>().enabled = false;
        }
    }
}
