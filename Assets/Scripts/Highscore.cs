using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Highscore
{
    private string playerName;
    private int score;
    private DateTime date_time;

    public string PlayerName { get => playerName; set => playerName = value; }
    public int Score { get => score; set => score = value; }
    public DateTime Date_time { get => date_time; set => date_time = value; }

    public Highscore(string playerName, int score, DateTime date_time)
    {
        this.playerName = playerName;
        this.score = score;
        this.date_time = date_time;
    }
}
