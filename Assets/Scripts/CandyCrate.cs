using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandyCrate : MonoBehaviour
{

    [SerializeField] private int _maxHealth = 100;
    private int _currentHealth;

    private void Awake()
    {
        _currentHealth = _maxHealth;
    }

    public void TakeDamage(int amount)
    {
        _currentHealth -= amount;
        Debug.Log("Current Candy:" + _currentHealth);
    }

}
