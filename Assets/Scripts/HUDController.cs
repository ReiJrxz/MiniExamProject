using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDController : MonoBehaviour
{
    [Header("Assign TMP labels or legacy UI Text")]
    [SerializeField] TMP_Text timerTMP, scoreTMP, victoryScoreTMP, gameOverScoreTMP;
    [SerializeField] Text timerLegacy, scoreLegacy;
    [Header("Panels (start inactive)")]
    [SerializeField] GameObject victoryPanel, gameOverPanel;

    void OnEnable()
    {
        if (GameManager.Instance == null) return;
        GameManager.Instance.TimeChanged += UpdateTimer;
        GameManager.Instance.ScoreChanged += UpdateScore;
        GameManager.Instance.StateChanged += UpdateState;
        UpdateTimer(GameManager.Instance.TimeRemaining);
        UpdateScore(GameManager.Instance.Score);
        UpdateState(GameManager.Instance.State);
    }

    void OnDisable()
    {
        if (GameManager.Instance == null) return;
        GameManager.Instance.TimeChanged -= UpdateTimer;
        GameManager.Instance.ScoreChanged -= UpdateScore;
        GameManager.Instance.StateChanged -= UpdateState;
    }

    void UpdateTimer(float seconds)
    {
        string s = "Time: " + Mathf.CeilToInt(seconds);
        if (timerTMP != null) timerTMP.text = s;
        if (timerLegacy != null) timerLegacy.text = s;
    }

    void UpdateScore(int score)
    {
        string s = "Score: " + score.ToString("N0");
        if (scoreTMP != null) scoreTMP.text = s;
        if (scoreLegacy != null) scoreLegacy.text = s;
        if (victoryScoreTMP != null) victoryScoreTMP.text = s;
        if (gameOverScoreTMP != null) gameOverScoreTMP.text = s;
    }

    void UpdateState(GameManager.GameState state)
    {
        if (victoryPanel != null) victoryPanel.SetActive(state == GameManager.GameState.Victory);
        if (gameOverPanel != null) gameOverPanel.SetActive(state == GameManager.GameState.GameOver);
    }
}
