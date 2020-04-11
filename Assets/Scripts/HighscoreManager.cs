using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class HighscoreManager
{
    public Action highscoreEvent;

    private string highscore_file = "high.score";
    private List<Highscore> highscores;

    public HighscoreManager()
    {
        if (File.Exists(Application.persistentDataPath + highscore_file))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + highscore_file, FileMode.Open);
            if (file.Length > 0)
            {
                file.Position = 0;
                highscores = (List<Highscore>)bf.Deserialize(file);
            }
            else
            {
                highscores = new List<Highscore>();
            }
        }
        else
        {
            File.Create(Application.persistentDataPath + highscore_file);
            highscores = new List<Highscore>();
        }
    }

    public void SaveHighscores()
    {
        if (File.Exists(Application.persistentDataPath + highscore_file))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + highscore_file, FileMode.Open);
            bf.Serialize(file, highscores);
        }
    }

    public void NewHighscore(string playerName, int score, DateTime date_time)
    {
        Highscore newHighscore = new Highscore(playerName, score, date_time);
        highscores.Add(newHighscore);
        highscores.Sort((s1, s2) => s2.Score.CompareTo(s1.Score));

        if (highscoreEvent != null)
        {
            highscoreEvent();
        }
    }

    public List<Highscore> GetHighscores()
    {
        return highscores;
    }

    public string GetHighscoresString(int topN)
    {
        string result = "";
        int i = 1;

        foreach (Highscore h in highscores)
        {
            result += i + ". " + h.PlayerName + ": " + h.Score + "\n";

            if (i >= topN)
            {
                break;
            }

            i++;
        }

        return result;
    }

    private void DebugOuput()
    {
        foreach (Highscore h in highscores)
        {
            Debug.Log(h.PlayerName + ": " + h.Score + " - " + h.Date_time.ToString());
        }
    }
}