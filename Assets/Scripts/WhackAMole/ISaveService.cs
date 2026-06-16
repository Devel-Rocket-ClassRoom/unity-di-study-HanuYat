using System;

public interface ISaveService
{
    event Action<int> Saved;

    int LoadHighScore();

    void SaveHighScore(int score);
}
