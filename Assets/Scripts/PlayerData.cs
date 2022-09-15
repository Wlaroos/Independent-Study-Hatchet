using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{

    private int _candyAmount;
    private int _maxHealth = 3;
    private int _currentHealth;

    private void Awake()
    {
        _currentHealth = _maxHealth;
    }

    public void Candy(int amount)
    {
        _candyAmount += amount;
    }
}
