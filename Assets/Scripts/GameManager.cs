using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        Menu,
        Playing,
        Dead
    }

    public Action startGameEvent;
    public HighscoreManager highscoreManager;

    [SerializeField]
    private PlayerController playerController = null;
    [SerializeField]
    private PlatformGenerator platformGenerator = null;

    //private GameState gameState;
    private string playerName;
    public int difficulty;
    private int score;
    private Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("playerName"))
        {
            playerName = PlayerPrefs.GetString("playerName");
        }
        else
        {
            playerName = "noname";
        }

        highscoreManager = new HighscoreManager();
        playerController.deathEvent += OnDeath;
        playerController.scoreEvent += UpdateScore;
        //gameState = GameState.Menu;
        cam = Camera.main.GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartGame()
    {
        if (startGameEvent != null)
        {
            startGameEvent();
        }
        //gameState = GameState.Playing;
        StartCoroutine(SetBGColor(new Color(1f, 0f, 0f), 50));
    }

    void OnDeath()
    {
        highscoreManager.NewHighscore(playerName, playerController.Score, DateTime.Now);
        highscoreManager.SaveHighscores();
        //gameState = GameState.Dead;
        StartCoroutine(SetBGColor(new Color(1f, 1f, 1f), 50));
    }

    void UpdateScore(int s)
    {
        score = s;
    }

    public int GetScore()
    {
        return score;
    }

    public void Menu()
    {
        //gameState = GameState.Menu;
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void SetPlayerName(string name)
    {
        playerName = name.ToLower();
        PlayerPrefs.SetString("playerName", playerName);
    }

    public string GetPlayerName()
    {
        return playerName;
    }

    public void SetDifficulty(int d)
    {
        difficulty = d;
        platformGenerator.SetDifficulty(d);
    }

    public float GetDifficultyFactor()
    {
        return ((float)difficulty - 5f) / 5f;
    }

    public IEnumerator SetBGColor(Color col, int steps)
    {
        Color current_col = cam.backgroundColor;
        for (int i = 1; i < steps; i++)
        {
            cam.backgroundColor = Color.Lerp(current_col, col, (float)i / (float)steps);
            yield return null;
        }
    }

    public void NextStage()
    {
        platformGenerator.NextStage();
    }
}