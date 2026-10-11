using UnityEngine;
using UnityEngine.SceneManagement;

namespace ZiddikJari
{
    /// <summary>One persistent audio manager for menu/game music and result SFX.</summary>
    public sealed class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance { get; private set; }

        [Header("Scene names (must match Build Settings)")]
        [SerializeField] private string mainMenuScene = "MainMenu";
        [SerializeField] private string level1Scene = "Level1";
        [SerializeField] private string level2Scene = "Level2";

        [Header("Music")]
        [SerializeField] private AudioSource musicSource;
        [SerializeField] private AudioClip mainMenuMusic;
        [SerializeField] private AudioClip level1Music;
        [SerializeField] private AudioClip level2Music;

        [Header("Result sound effects")]
        [SerializeField] private AudioSource sfxSource;
        [SerializeField] private AudioClip victorySfx;
        [SerializeField] private AudioClip loseSfx;

        private const string MusicVolumeKey = "MusicVolume";
        private const string SfxVolumeKey = "SfxVolume";

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            if (musicSource == null || sfxSource == null)
            {
                Debug.LogError("AudioManager needs separate Music and SFX AudioSources assigned.", this);
                return;
            }

            musicSource.loop = true;
            musicSource.playOnAwake = false;
            sfxSource.playOnAwake = false;
            musicSource.volume = PlayerPrefs.GetFloat(MusicVolumeKey, 1f);
            sfxSource.volume = PlayerPrefs.GetFloat(SfxVolumeKey, 1f);
            SceneManager.sceneLoaded += OnSceneLoaded;
            PlayMusicForScene(SceneManager.GetActiveScene().name);
        }

        private void OnDestroy()
        {
            if (Instance != this) return;
            SceneManager.sceneLoaded -= OnSceneLoaded;
            Instance = null;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            PlayMusicForScene(scene.name);
        }

        private void PlayMusicForScene(string sceneName)
        {
            AudioClip nextClip = null;
            if (sceneName == mainMenuScene) nextClip = mainMenuMusic;
            else if (sceneName == level1Scene) nextClip = level1Music;
            else if (sceneName == level2Scene) nextClip = level2Music;

            if (musicSource == null || musicSource.clip == nextClip) return;
            musicSource.Stop();
            musicSource.clip = nextClip;
            if (nextClip != null) musicSource.Play();
        }

        public void PlayVictorySfx() => PlaySfx(victorySfx);
        public void PlayLoseSfx() => PlaySfx(loseSfx);

        private void PlaySfx(AudioClip clip)
        {
            if (clip == null)
            {
                Debug.LogWarning("A result SFX clip has not been assigned.", this);
                return;
            }
            if (sfxSource != null) sfxSource.PlayOneShot(clip);
        }

        public void SetMusicVolume(float value)
        {
            value = Mathf.Clamp01(value);
            if (musicSource != null) musicSource.volume = value;
            PlayerPrefs.SetFloat(MusicVolumeKey, value);
        }

        public void SetSfxVolume(float value)
        {
            value = Mathf.Clamp01(value);
            if (sfxSource != null) sfxSource.volume = value;
            PlayerPrefs.SetFloat(SfxVolumeKey, value);
        }
    }
}
