using System;
using UnityEngine;
using VContainer.Unity;

public sealed class MyGameDirector : IRoundService, IStartable, ITickable, IDisposable
{
    private readonly IScoreService m_Score;
    private readonly ISaveService m_Save;
    private readonly MyGameConfig m_Config;

    public bool IsPlaying { get; private set; }
    public float TimeLeft { get; private set; }
    public int HighScore { get; private set; }

    public event Action RoundStarted;
    public event Action<int> RoundEnded;
    public event Action<float> TimeChanged;
    public event Action<int> HighScoreChanged;

    public MyGameDirector(IScoreService score, ISaveService save, MyGameConfig config)
    {
        m_Score = score;
        m_Save = save;
        m_Config = config;
    }

    public void Start()
    {
        HighScore = m_Save.LoadHighScore();
        HighScoreChanged?.Invoke(HighScore);
        StartRound();
    }

    public void StartRound()
    {
        m_Score.Restore(0);
        TimeLeft = m_Config.RoundDuration;
        IsPlaying = true;

        TimeChanged?.Invoke(TimeLeft);
        RoundStarted?.Invoke();
        Debug.Log($"[GameDirector] 라운드 시작 — 제한 시간 {TimeLeft:F0}초");
    }

    public void Tick()
    {
        if (!IsPlaying)
            return;

        TimeLeft -= Time.deltaTime;
        if (TimeLeft <= 0f)
        {
            TimeLeft = 0f;
            TimeChanged?.Invoke(TimeLeft);
            EndRound();
            return;
        }

        TimeChanged?.Invoke(TimeLeft);
    }

    private void EndRound()
    {
        IsPlaying = false;

        int finalScore = m_Score.CurrentScore;
        if (finalScore > HighScore)
        {
            HighScore = finalScore;
            m_Save.SaveHighScore(HighScore);
            HighScoreChanged?.Invoke(HighScore);
            Debug.Log($"[GameDirector] 신기록! {HighScore}점 저장");
        }

        RoundEnded?.Invoke(finalScore);
        Debug.Log($"[GameDirector] 라운드 종료 — 이번 점수 {finalScore}점");
    }

    public void Dispose()
    {
        if (m_Score.CurrentScore > HighScore)
            m_Save.SaveHighScore(m_Score.CurrentScore);
    }
}
