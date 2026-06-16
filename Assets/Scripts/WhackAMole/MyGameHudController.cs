using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class MyGameHudController : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI m_ScoreText;

    [SerializeField]
    private TextMeshProUGUI m_HighScoreText;

    [SerializeField]
    private TextMeshProUGUI m_TimeText;

    [SerializeField]
    private TextMeshProUGUI m_StatusText;

    [SerializeField]
    private Button m_RestartButton;

    private IScoreService m_Score;
    private IRoundService m_Round;

    [Inject]
    public void Construct(IScoreService score, IRoundService round)
    {
        m_Score = score;
        m_Round = round;
    }

    private void Start()
    {
        if (m_Score == null || m_Round == null)
        {
            SetStatus("주입 실패 — LifetimeScope 등록을 확인하세요.");
            return;
        }

        m_Score.ScoreChanged += OnScoreChanged;
        m_Round.TimeChanged += OnTimeChanged;
        m_Round.HighScoreChanged += OnHighScoreChanged;
        m_Round.RoundStarted += OnRoundStarted;
        m_Round.RoundEnded += OnRoundEnded;

        if (m_RestartButton != null)
            m_RestartButton.onClick.AddListener(OnRestartClicked);

        // 현재 상태로 초기 표시.
        OnScoreChanged(m_Score.CurrentScore);
        OnHighScoreChanged(m_Round.HighScore);
        OnTimeChanged(m_Round.TimeLeft);
        SetStatus("두더지를 클릭하세요!");
    }

    private void OnDestroy()
    {
        if (m_Score != null)
            m_Score.ScoreChanged -= OnScoreChanged;
        if (m_Round != null)
        {
            m_Round.TimeChanged -= OnTimeChanged;
            m_Round.HighScoreChanged -= OnHighScoreChanged;
            m_Round.RoundStarted -= OnRoundStarted;
            m_Round.RoundEnded -= OnRoundEnded;
        }
    }

    private void OnScoreChanged(int score)
    {
        if (m_ScoreText != null)
            m_ScoreText.text = $"점수: {score}";
    }

    private void OnHighScoreChanged(int high)
    {
        if (m_HighScoreText != null)
            m_HighScoreText.text = $"최고: {high}";
    }

    private void OnTimeChanged(float timeLeft)
    {
        if (m_TimeText != null)
            m_TimeText.text = $"시간: {Mathf.CeilToInt(timeLeft)}";
    }

    private void OnRoundStarted()
    {
        SetStatus("두더지를 클릭하세요!");
    }

    private void OnRoundEnded(int finalScore)
    {
        SetStatus($"라운드 종료 — {finalScore}점. '다시 시작'을 눌러 새 라운드를 시작하세요.");
    }

    private void OnRestartClicked()
    {
        m_Round.StartRound();
    }

    private void SetStatus(string message)
    {
        if (m_StatusText != null)
            m_StatusText.text = message;
    }
}
