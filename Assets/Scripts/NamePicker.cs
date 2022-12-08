using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NamePicker : MonoBehaviour
{

    [SerializeField] GameObject highScoreSetter;

    [SerializeField] AudioClip[] buttonSFX;

    private Button Button1Up;
    private Button Button1Down;
    private Button Button2Up;
    private Button Button2Down;
    private Button Button3Up;
    private Button Button3Down;

    private Button ReturnButton;

    private Image BG;

    private char Letter1 = 'a';
    private char Letter2 = 'a';
    private char Letter3 = 'a';


    private TMP_Text Text1;
    private TMP_Text Text2;
    private TMP_Text Text3;

    private int score;

    private bool fade = false;

    private void Awake()
    {
        BG = GameObject.Find("BG").GetComponent<Image>();

        Button1Down = GameObject.Find("Button (2)").GetComponent<Button>();
        Button2Down = GameObject.Find("Button (4)").GetComponent<Button>();
        Button3Down = GameObject.Find("Button (6)").GetComponent<Button>();
        Button1Up = GameObject.Find("Button (1)").GetComponent<Button>();
        Button2Up = GameObject.Find("Button (3)").GetComponent<Button>();
        Button3Up = GameObject.Find("Button (5)").GetComponent<Button>();

        ReturnButton = GameObject.Find("Button").GetComponent<Button>();

        Text1 = GameObject.Find("Letter01").GetComponent<TMP_Text>();
        Text2 = GameObject.Find("Letter02").GetComponent<TMP_Text>();
        Text3 = GameObject.Find("Letter03").GetComponent<TMP_Text>();

        Button1Down.onClick.AddListener(DecrementChar1);
        Button2Down.onClick.AddListener(DecrementChar2);
        Button3Down.onClick.AddListener(DecrementChar3);
        Button1Up.onClick.AddListener(IncrementChar1);
        Button2Up.onClick.AddListener(IncrementChar2);
        Button3Up.onClick.AddListener(IncrementChar3);

        ReturnButton.onClick.AddListener(SendScore);

        gameObject.SetActive(false);
    }

    private void Update()
    {
        if(fade == true)
        {
            BG.color = new Color(BG.color.r, BG.color.g, BG.color.b, BG.color.a + 1f * Time.deltaTime);
        }
    }

    private void DecrementChar1()
    {
        Letter1--;
        AudioHelper.PlayClip2D(buttonSFX[1], 1);
        if ((byte)Letter1 < 97)
        {
            Letter1 = 'z';
        }
        Text1.text = Letter1.ToString();
    }

    private void IncrementChar1()
    {
        Letter1++;
        AudioHelper.PlayClip2D(buttonSFX[0], 1);
        if ((byte)Letter1 > 122)
        {
            Letter1 = 'a';
        }
        Text1.text = Letter1.ToString();
    }

    private void DecrementChar2()
    {
        Letter2--;
        AudioHelper.PlayClip2D(buttonSFX[1], 1);
        if ((byte)Letter2 < 97)
        {
            Letter2 = 'z';
        }
        Text2.text = Letter2.ToString();
    }

    private void IncrementChar2()
    {
        Letter2++;
        AudioHelper.PlayClip2D(buttonSFX[0], 1);
        if ((byte)Letter2 > 122)
        {
            Letter2 = 'a';
        }
        Text2.text = Letter2.ToString();
    }

    private void DecrementChar3()
    {
        Letter3--;
        AudioHelper.PlayClip2D(buttonSFX[1], 1);
        if ((byte)Letter3 < 97)
        {
            Letter3 = 'z';
        }
        Text3.text = Letter3.ToString();
    }

    private void IncrementChar3()
    {
        Letter3++;
        AudioHelper.PlayClip2D(buttonSFX[0], 1);
        if ((byte)Letter3 > 122)
        {
            Letter3 = 'a';
        }
        Text3.text = Letter3.ToString();
    }

    private void SendScore()
    {
        string str = "";
        str += Letter1.ToString();
        str += Letter2.ToString();
        str += Letter3.ToString();
        HighScoreManager.Instance.SaveHighScore(str, score);
        highScoreSetter.SetActive(true);
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        fade = true;
        score = GameScore.Instance.GetScore();
    }
}
