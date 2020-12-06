using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DeadUI : MonoBehaviour
{
    public TextMeshProUGUI scoreText;

    // Start is called before the first frame update
    void Start()
    {
        FindObjectOfType<PlayerController>().deathEvent += Show;
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

    void SetScore()
    {
        int score = FindObjectOfType<GameManager>().GetScore();
        scoreText.text = "final score\n" + score;
    }
}