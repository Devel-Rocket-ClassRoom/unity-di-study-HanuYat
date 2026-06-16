using System;

public interface IRoundService
{
    bool IsPlaying { get; }

    float TimeLeft { get; }

    int HighScore { get; }

    event Action RoundStarted;

    event Action<int> RoundEnded;

    event Action<float> TimeChanged;

    event Action<int> HighScoreChanged;

    void StartRound();
}
