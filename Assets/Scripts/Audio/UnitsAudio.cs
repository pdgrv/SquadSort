using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitsAudio : Audio
{
    public static UnitsAudio Instance = null;

    [SerializeField] private AudioClip _askingYes;
    [SerializeField] private List<AudioClip> _agreements;
    [SerializeField] private AudioClip _disagreement;
    [SerializeField] private AudioClip _battleStance;
    [SerializeField] private AudioClip _enchantWhoosh;
    [SerializeField] private float _whooshDelay = 0.1f;
    [SerializeField] private AudioClip _celebrate;

    [Header("Combat")]
    [SerializeField] private List<AudioClip> _swordsAttack;
    [SerializeField] private int _attackSoundChance;
    [SerializeField] private AudioClip _bowShoot;
    [SerializeField] private int _bowShootSoundChance;
    [SerializeField] private AudioClip _applyDamage;
    [SerializeField] private int _applyDamageSoundChance;
    [SerializeField] private AudioClip _die;

    private void Start()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance == this)
            Destroy(gameObject);
    }

    public void Attack()
    {
        PlayOneShot(_swordsAttack[Random.Range(0, _swordsAttack.Count)], volumeScale:Random.Range(0.7f,0.9f));
    }

    public void Shoot()
    {
        PlayOneShot(_bowShoot);
    }

    public void ApplyDamage()
    {
        PlayOneShot(_applyDamage,_applyDamageSoundChance);
    }

    public void Die()
    {
        PlayOneShot(_die);
    }

    public void Select()
    {
        PlayOneShot(_askingYes);
    }

    public void Agreement()
    {
        PlayOneShot(_agreements[Random.Range(0, _agreements.Count)]);
    }

    public void Disagreement()
    {
        PlayOneShot(_disagreement);
    }
    public void Complete()
    {
        PlayOneShot(_battleStance);
        PlayOneShot(_enchantWhoosh, _whooshDelay);
    }

    public void Celebate()
    {
        AudioSource.clip = _celebrate;
        AudioSource.loop = true;
        AudioSource.Play();
    }
}
