using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandyCrate : MonoBehaviour
{
    ParticleSystem ps;
    [SerializeField] private int _maxHealth = 100;
    private int _currentHealth;
    private SpriteRenderer _hpsr;

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

        Debug.Log("Current Candy:" + _currentHealth);
        ps.Play();
    }

}
