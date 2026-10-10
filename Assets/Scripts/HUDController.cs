
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDController : MonoBehaviour
{
    [Header("Assign TMP labels or legacy UI Text")]
    [SerializeField] TMP_Text timerTMP;
    [SerializeField] TMP_Text scoreTMP;
    [SerializeField] TMP_Text victoryScoreTMP;
    [SerializeField] TMP_Text gameOverScoreTMP;

    [SerializeField] Text timerLegacy;
    [SerializeField] Text scoreLegacy;

    [Header("Panels")]
    [SerializeField] GameObject victoryPanel;
    [SerializeField] GameObject gameOverPanel;

    GameManager subscribedManager;

    void Start()
    {
        ConnectToGameManager();
    }

    void OnDestroy()
    {
        DisconnectFromGameManager();
    }

    void ConnectToGameManager()
    {
        if (subscribedManager != null) return;

        if (GameManager.Instance == null)
        {
            Debug.LogError(
                "HUDController: GameManager.Instance is null. " +
                "Check that GameManager exists in the scene."
            );
            return;
        }

        subscribedManager = GameManager.Instance;

        subscribedManager.TimeChanged += UpdateTimer;
        subscribedManager.ScoreChanged += UpdateScore;
        subscribedManager.StateChanged += UpdateState;

        UpdateTimer(subscribedManager.TimeRemaining);
        UpdateScore(subscribedManager.Score);
        UpdateState(subscribedManager.State);

        Debug.Log("HUDController connected to GameManager.");
    }

    void DisconnectFromGameManager()
    {
        if (subscribedManager == null) return;

        subscribedManager.TimeChanged -= UpdateTimer;
        subscribedManager.ScoreChanged -= UpdateScore;
        subscribedManager.StateChanged -= UpdateState;

        subscribedManager = null;
    }

    void UpdateTimer(float seconds)
    {
        string value = "Time: " + Mathf.CeilToInt(seconds);

        if (timerTMP != null) timerTMP.text = value;
        if (timerLegacy != null) timerLegacy.text = value;
    }

    void UpdateScore(int score)
    {
        string value = "Score: " + score.ToString("N0");

        if (scoreTMP != null) scoreTMP.text = value;
        if (scoreLegacy != null) scoreLegacy.text = value;
        if (victoryScoreTMP != null) victoryScoreTMP.text = value;
        if (gameOverScoreTMP != null) gameOverScoreTMP.text = value;
    }

    void UpdateState(GameManager.GameState state)
    {
        if (victoryPanel != null)
            victoryPanel.SetActive(state == GameManager.GameState.Victory);

        if (gameOverPanel != null)
            gameOverPanel.SetActive(state == GameManager.GameState.GameOver);
    }
}
