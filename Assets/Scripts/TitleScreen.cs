using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour
{
    int nextSceneIndex;

    private void Awake()
    {
        nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return))
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        if(Input.GetKeyDown(KeyCode.Z))
        {
            HighScoreManager.Instance.ClearLeaderBoard();
        }
    }
}
