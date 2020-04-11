using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextHighscores : MonoBehaviour
{
    public HighscoreManager highscoreManager;

    // Start is called before the first frame update
    void Start()
    {
        highscoreManager = FindObjectOfType<GameManager>().highscoreManager;
        highscoreManager.highscoreEvent += UpdateText;
        GetComponent<TextMeshProUGUI>().text = highscoreManager.GetHighscoresString(5);
    }

    public void UpdateText()
    {
        GetComponent<TextMeshProUGUI>().text = highscoreManager.GetHighscoresString(5);
    }
}
