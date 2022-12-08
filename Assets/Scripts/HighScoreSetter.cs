using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class HighScoreSetter : MonoBehaviour
{
    private TMP_Text[] Scores = new TMP_Text[10];
    private TMP_Text[] Names = new TMP_Text[10];

    int prevSceneIndex;

    private void Awake()
    {
        prevSceneIndex = SceneManager.GetActiveScene().buildIndex - 1;

        for (int i = 0; i < 10; i++)
        {
            Scores[i] = GameObject.Find("Score0" + (i + 1).ToString()).GetComponent<TMP_Text>();
        }

        for (int i = 0; i < 10; i++)
        {
            Names[i] = GameObject.Find("Name0" + (i + 1).ToString()).GetComponent<TMP_Text>();
        }

        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            SceneManager.LoadScene(prevSceneIndex);
        }
    }

    private void OnEnable()
    {
        List<Scores> temp = HighScoreManager.Instance.GetHighScore();
        for (int i = 0; i < temp.Count; i++)
        {
            Names[i].text = temp[i].name.ToString();
            Scores[i].text = temp[i].score.ToString();
        }
    }

}
