using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PauseUI : MonoBehaviour
{
    public TextMeshProUGUI scoreText;

    void SetScore()
    {
        int score = FindObjectOfType<GameManager>().GetScore();
        scoreText.text = "score\n" + score;
    }

    // Start is called before the first frame update
    void Start()
    {
        FindObjectOfType<PlayerController>().pauseEvent += Show;
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Show()
    {
        gameObject.SetActive(true);
        SetScore();
    }
}