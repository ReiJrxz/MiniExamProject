using UnityEngine;
using UnityEngine.SceneManagement;

namespace ZiddikJari
{
    /// <summary>Connect this component's public methods to the Main Menu UI Buttons.</summary>
    public sealed class MainMenuUIManager : MonoBehaviour
    {
        [Header("Scene names (must match Build Settings)")]
        [SerializeField] private string levelSelectScene = "LevelSelect";
        [SerializeField] private string level1Scene = "Level1";
        [SerializeField] private string level2Scene = "Level2";

        [Header("Menu panels")]
        [SerializeField] private GameObject mainPanel;
        [SerializeField] private GameObject optionsPanel;

        private void Start()
        {
            ShowMain();
        }

        public void StartGame()
        {
            LoadScene(levelSelectScene);
        }

        public void OpenOptions()
        {
            if (mainPanel != null) mainPanel.SetActive(false);
            if (optionsPanel != null) optionsPanel.SetActive(true);
        }

        public void CloseOptions()
        {
            ShowMain();
        }

        public void ShowMain()
        {
            if (mainPanel != null) mainPanel.SetActive(true);
            if (optionsPanel != null) optionsPanel.SetActive(false);
        }

        public void SelectLevel1() => LoadScene(level1Scene);
        public void SelectLevel2() => LoadScene(level2Scene);
        public void BackToMainMenu() => LoadScene("MainMenu");

        public void QuitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        private static void LoadScene(string sceneName)
        {
            if (string.IsNullOrWhiteSpace(sceneName))
            {
                Debug.LogError("A target scene name is empty.");
                return;
            }
            SceneManager.LoadScene(sceneName);
        }
    }
}
