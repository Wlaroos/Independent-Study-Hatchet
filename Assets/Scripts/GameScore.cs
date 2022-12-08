using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameScore : MonoBehaviour
{

    public static GameScore Instance { get; private set;  }

    private int scoreTotal;

    private Text scoreText;

    void Awake()
    {

        if(Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        scoreText = GetComponent<Text>();
    }

    public void AddScore(int amount)
    {
        scoreTotal += amount;
        //scoreText.text = (scoreTotal.ToString().PadLeft(7, '0'));
    }

    public int GetScore()
    {
        return scoreTotal;
    }

}
