
using UnityEngine;
using TMPro;

namespace ZiddikJari
{
    public class HUDController : MonoBehaviour
    {
        [Header("Timer")]
        [SerializeField] private TMP_Text timerTMP;

        [SerializeField] private Color normalTimerColor = Color.white;
        [SerializeField] private Color warningTimerColor = new Color(1f, 0.55f, 0f);
        [SerializeField] private Color criticalTimerColor = Color.red;

        [SerializeField] private float warningThreshold = 15f;
        [SerializeField] private float criticalThreshold = 5f;

        [Header("Score")]
        [SerializeField] private TMP_Text scoreTMP;
        [SerializeField] private TMP_Text victoryScoreTMP;
        [SerializeField] private TMP_Text gameOverScoreTMP;

        [Header("Panels")]
        [SerializeField] private GameObject victoryPanel;
        [SerializeField] private GameObject gameOverPanel;

        private GameManager subscribedManager;

        private void Start()
        {
            ConnectToGameManager();
        }

        private void OnDestroy()
        {
            DisconnectFromGameManager();
        }

        private void ConnectToGameManager()
        {
            if (subscribedManager != null)
                return;

            if (GameManager.Instance == null)
            {
                Debug.LogError(
                    "HUDController: GameManager.Instance is null. " +
                    "Check that GameManager exists in the scene.",
                    this
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

            Debug.Log("HUDController connected to GameManager.", this);
        }

        private void DisconnectFromGameManager()
        {
            if (subscribedManager == null)
                return;

            subscribedManager.TimeChanged -= UpdateTimer;
            subscribedManager.ScoreChanged -= UpdateScore;
            subscribedManager.StateChanged -= UpdateState;

            subscribedManager = null;
        }

        private void UpdateTimer(float seconds)
        {
            if (timerTMP == null)
                return;

            seconds = Mathf.Max(0f, seconds);

            // แสดงวินาทีและเศษวินาที 2 หลัก เช่น 09.45
            int wholeSeconds = Mathf.FloorToInt(seconds);
            int centiseconds = Mathf.FloorToInt(
                (seconds - wholeSeconds) * 100f
            );

            timerTMP.text = $"{wholeSeconds:00}.{centiseconds:00}";

            // 5 วินาทีหรือน้อยกว่า: สีแดง
            if (seconds <= criticalThreshold)
            {
                timerTMP.color = criticalTimerColor;
            }
            // 15 วินาทีหรือน้อยกว่า: สีส้ม
            else if (seconds <= warningThreshold)
            {
                timerTMP.color = warningTimerColor;
            }
            // เวลาปกติ
            else
            {
                timerTMP.color = normalTimerColor;
            }
        }

        private void UpdateScore(int score)
        {
            string value = "Score: " + score.ToString("N0");

            if (scoreTMP != null)
                scoreTMP.text = value;

            if (victoryScoreTMP != null)
                victoryScoreTMP.text = value;

            if (gameOverScoreTMP != null)
                gameOverScoreTMP.text = value;
        }

        private void UpdateState(GameManager.GameState state)
        {
            if (victoryPanel != null)
                victoryPanel.SetActive(
                    state == GameManager.GameState.Victory
                );

            if (gameOverPanel != null)
                gameOverPanel.SetActive(
                    state == GameManager.GameState.GameOver
                );
        }
    }
}
