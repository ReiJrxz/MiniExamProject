using UnityEngine;
using UnityEngine.SceneManagement;

namespace ZiddikJari
{
public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }
        public enum GameState { Playing, Victory, GameOver }

        [SerializeField] float startingTime = 30f;
        [SerializeField] int startingScore = 0;
        [SerializeField] int scoreToWin = 10000;
        [SerializeField] bool requireMoreThanTarget = true;
        [SerializeField] string mainMenuSceneName = "MainMenu";

        public GameState State { get; private set; } = GameState.Playing;
        public float TimeRemaining { get; private set; }
        public int Score { get; private set; }
        public bool IsPlaying => State == GameState.Playing;
        public event System.Action<float> TimeChanged;
        public event System.Action<int> ScoreChanged;
        public event System.Action<GameState> StateChanged;

        void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            Instance = this; Time.timeScale = 1f;
            TimeRemaining = startingTime; Score = startingScore;
        }

        void Start()
        {
            TimeChanged?.Invoke(TimeRemaining); ScoreChanged?.Invoke(Score); StateChanged?.Invoke(State);
        }

        void Update()
        {
            if (!IsPlaying) return;
            TimeRemaining = Mathf.Max(0f, TimeRemaining - Time.deltaTime);
            TimeChanged?.Invoke(TimeRemaining);
            if (TimeRemaining <= 0f) ResolveTimeUp();
        }

        public void AddTime(float amount)
        {
            if (!IsPlaying) return;
            TimeRemaining = Mathf.Max(0f, TimeRemaining + amount);
            TimeChanged?.Invoke(TimeRemaining);
            if (TimeRemaining <= 0f) ResolveTimeUp();
        }

        public void AddScore(int amount)
        {
            if (!IsPlaying) return;
            Score += amount; ScoreChanged?.Invoke(Score);
        }

        void ResolveTimeUp()
        {
            bool won = requireMoreThanTarget ? Score > scoreToWin : Score >= scoreToWin;
            Finish(won ? GameState.Victory : GameState.GameOver);
        }

        public void PlayerFell() { if (IsPlaying) Finish(GameState.GameOver); }

        void Finish(GameState result)
        {
            if (!IsPlaying) return;
            State = result; StateChanged?.Invoke(State);
            foreach (TankControl c in FindObjectsOfType<TankControl>()) c.SetControlsEnabled(false);
        }

        public void RestartLevel()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void LoadMainMenu()
        {
            Time.timeScale = 1f;
            if (!string.IsNullOrWhiteSpace(mainMenuSceneName)) SceneManager.LoadScene(mainMenuSceneName);
            else Debug.LogWarning("Set Main Menu Scene Name on GameManager.");
        }
    }
}
