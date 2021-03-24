using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAudio : Audio
{
    public static ZombieAudio Instance = null;

    [SerializeField] private List<AudioClip> _voices;
    [SerializeField] private float _averageVoiceTimer;

    [Header("Combat")]
    [SerializeField] private List<AudioClip> _attacks;
    [SerializeField] private int _attackSoundChance;
    [SerializeField] private List<AudioClip> _applyDamage;
    [SerializeField] private int _applyDamageSoundChance;
    [SerializeField] private List<AudioClip> _die;
    [SerializeField] private int _dieSoundChance;

    private int _zombiesCount = 0;
    private float _realTimer;
    private float _timer;

    private void Start()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance == this)
            Destroy(gameObject);

        _realTimer = 2f;
    }

    private void FixedUpdate()
    {
        if (_zombiesCount == 0)
            return;

        _timer += Time.deltaTime;

        if (_timer > _realTimer)
        {
            Voice();

            _realTimer = _averageVoiceTimer / _zombiesCount;
            _timer = 0;
        }
    }

    public void Attack()
    {
        PlayOneShot(_attacks[Random.Range(0, _attacks.Count)], _attackSoundChance);
    }

    public void ApplyDamage()
    {
        PlayOneShot(_applyDamage[Random.Range(0, _applyDamage.Count)], _applyDamageSoundChance);
    }

    public void Die()
    {
        PlayOneShot(_die[Random.Range(0, _die.Count)], _dieSoundChance);
    }

    public void Voice()
    {
        PlayOneShot(_voices[Random.Range(0, _voices.Count)]);
    }

    public void UpdateZombieCount(int count)
    {
        _zombiesCount = count;
    }
}
