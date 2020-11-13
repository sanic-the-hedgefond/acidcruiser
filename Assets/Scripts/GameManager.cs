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

    public Color color_BGmenu;
    public Color color_BGgame;

    [SerializeField]
    private PlayerController playerController = null;
    [SerializeField]
    private PlatformGenerator platformGenerator = null;
    [SerializeField]
    public Background background = null;

    public int hitsTilDeath = 5;

    public AudioSource audioGameStart;
    public AudioSource audioPlatformOutOfFrame;
    public AudioSource audioCoin;

    //private GameState gameState;
    private string playerName;
    private float sensitivity;
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

        if (PlayerPrefs.HasKey("sensitivity"))
        {
            sensitivity = PlayerPrefs.GetFloat("sensitivity");
        }
        else
        {
            sensitivity = 1f;
        }

        highscoreManager = new HighscoreManager();
        playerController.deathEvent += OnDeath;
        playerController.scoreEvent += UpdateScore;
        //gameState = GameState.Menu;
        cam = Camera.main.GetComponent<Camera>();
    }

    public void StartGame()
    {
        if (startGameEvent != null)
        {
            startGameEvent();
        }
        //gameState = GameState.Playing;
        audioGameStart.Play();
        //StartCoroutine(SetBGColor(color_BGgame, 50));
        //StartCoroutine(background.SetGridColor(new Color(191/255f, 82/255f, 167/255f, 1f) * 2f));
    }

    void OnDeath()
    {
        //StartCoroutine(SetBGColor(color_BGmenu, 50));
        //StartCoroutine(background.SetGridColor(new Color(255 / 255f, 255 / 255f, 255 / 255f, 1f) * 1.5f));
        highscoreManager.NewHighscore(playerName, playerController.Score, DateTime.Now);
        highscoreManager.SaveHighscores();
        //gameState = GameState.Dead;
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

    public void SetSensitivity(float s)
    {
        sensitivity = s;
        PlayerPrefs.SetFloat("sensitivity", sensitivity);
    }

    public float GetSensitivity()
    {
        return sensitivity;
    }

    public void Pause()
    {
        Time.timeScale = 0;
    }

    public void Unpause()
    {
        Time.timeScale = 1;
    }
}