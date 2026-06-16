using System;
using UnityEngine;

public sealed class MyPlayerPrefsSaveService : ISaveService
{
    private const string HighScoreKey = "DIStudy.WhackAMole.HighScore";

    public event Action<int> Saved;

    public int LoadHighScore()
    {
        return PlayerPrefs.GetInt(HighScoreKey, 0);
    }

    public void SaveHighScore(int score)
    {
        PlayerPrefs.SetInt(HighScoreKey, score);
        PlayerPrefs.Save();
        Saved?.Invoke(score);
    }
}
