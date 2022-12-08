using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CandyCrate : MonoBehaviour
{
    public event Action CrateDamage;

    public int _maxHealth = 100;
    public int _currentHealth;

    [SerializeField] HUDController _HUDRef;
    [SerializeField] PlayerController __playerRef;

    private SpriteRenderer _hpsr;
    ParticleSystem ps;

    private void Awake()
    {
        _hpsr = transform.GetChild(0).GetComponent<SpriteRenderer>();
        ps = GetComponent<ParticleSystem>();
        _currentHealth = _maxHealth;
    }

    public void TakeDamage(int amount)
    {
        _currentHealth -= amount;

        _hpsr.transform.localScale = new Vector3((float)_currentHealth / (float)_maxHealth * 1.5f, .15f, 1);

        ps.Play();

        CrateDamage?.Invoke();

        if(_currentHealth <= 0)
        {
            Lose();
        }
    }

    private void Lose()
    {
        __playerRef.DecreaseHealth(100,0);
    }
}
