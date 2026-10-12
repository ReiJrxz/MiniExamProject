using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

namespace ZiddikJari
{
    public sealed class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance { get; private set; }

        [Header("Scene names")]
        [SerializeField] private string mainMenuScene = "MainMenu";
        [SerializeField] private string lobbyScene = "Lobby";
        [SerializeField] private string level1Scene = "Level1";
        [SerializeField] private string level2Scene = "Level2";

        [Header("Music")]
        [SerializeField] private AudioSource musicSource;
        [SerializeField] private AudioClip mainMenuMusic;
        [SerializeField] private AudioClip lobbyMusic;
        [SerializeField] private AudioClip level1Music;
        [SerializeField] private AudioClip level2Music;

        [Header("Audio Mixer Groups")]
        [SerializeField] private AudioMixerGroup musicGroup;
        [SerializeField] private AudioMixerGroup sfxGroup;
        [SerializeField] private AudioMixerGroup uiGroup;

        [Header("Mixer parameters")]
        [SerializeField] private AudioMixer audioMixer;
        [SerializeField] private string masterVolumeParameter = "MasterVolume";
        [SerializeField] private string musicVolumeParameter = "MusicVolume";
        [SerializeField] private string sfxMasterVolumeParameter = "SfxMasterVolume";
        [SerializeField] private string sfxVolumeParameter = "SfxVolume";
        [SerializeField] private string uiVolumeParameter = "UIVolume";

        [Header("Sound Effects")]
        [SerializeField] private AudioSource sfxSource;
        [SerializeField] private AudioSource uiSource;
        [SerializeField] private AudioClip victorySfx;
        [SerializeField] private AudioClip loseSfx;
        [SerializeField] private AudioClip jumpSfx;
        [SerializeField] private AudioClip obstacleHitSfx;
        [SerializeField] private AudioClip itemPickupSfx;

        private const string MusicPrefsKey = "MusicVolume";
        private const string SfxPrefsKey = "SfxVolume";
        private const string UiPrefsKey = "UIVolume";

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
                Debug.LogError(
                    "AudioManager needs Music Source and SFX Source assigned.",
                    this
                );
                return;
            }

            musicSource.outputAudioMixerGroup = musicGroup;
            sfxSource.outputAudioMixerGroup = sfxGroup;

            if (uiSource != null)
                uiSource.outputAudioMixerGroup = uiGroup;

            musicSource.loop = true;
            musicSource.playOnAwake = false;
            sfxSource.playOnAwake = false;

            if (uiSource != null)
                uiSource.playOnAwake = false;

            SetMasterVolume(PlayerPrefs.GetFloat("MasterVolume", 1f));
            SetMusicVolume(PlayerPrefs.GetFloat("MusicVolume", 1f));
            SetSfxMasterVolume(PlayerPrefs.GetFloat("SfxMasterVolume", 1f));
            SetSfxVolume(PlayerPrefs.GetFloat("SfxVolume", 1f));
            SetUIVolume(PlayerPrefs.GetFloat("UIVolume", 1f));

            SceneManager.sceneLoaded += OnSceneLoaded;
            PlayMusicForScene(SceneManager.GetActiveScene().name);
        }

        private void OnDestroy()
        {
            if (Instance != this)
                return;

           
            Instance = null;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            PlayMusicForScene(scene.name);
        }

        private void PlayMusicForScene(string sceneName)
        {
            AudioClip nextClip = null;

            if (sceneName == mainMenuScene)
                nextClip = mainMenuMusic;
            else if (sceneName == lobbyScene)
                nextClip = lobbyMusic;
            else if (sceneName == level1Scene)
                nextClip = level1Music;
            else if (sceneName == level2Scene)
                nextClip = level2Music;

            if (musicSource == null || musicSource.clip == nextClip)
                return;

            musicSource.Stop();
            musicSource.clip = nextClip;

            if (nextClip != null)
                musicSource.Play();
        }

        public void PlayVictorySfx()
        {
            PlaySfx(victorySfx);
        }

        public void PlayLoseSfx()
        {
            PlaySfx(loseSfx);
        }

        public void PlayJumpSfx()
        {
            PlaySfx(jumpSfx);
        }
        public void PlayObstacleHitSfx()
        {
            PlaySfx(obstacleHitSfx);
        }
        public void PlayItemPickupSfx(AudioClip clip)
        {
            if (clip == null)
            {
                Debug.LogWarning("Pickup SFX is not assigned.");
                return;
            }

            if (sfxSource == null)
            {
                Debug.LogError("SFX Source is not assigned.");
                return;
            }

            sfxSource.PlayOneShot(clip);
        }
        public void PlayUISfx(AudioClip clip)
        {
            if (clip == null)
            {
                Debug.LogWarning("UI SFX clip has not been assigned.", this);
                return;
            }

            if (uiSource != null)
                uiSource.PlayOneShot(clip);
            else
                Debug.LogWarning("UI Source has not been assigned.", this);
        }

        private void PlaySfx(AudioClip clip)
        {
            if (clip == null)
            {
                Debug.LogWarning("Result SFX clip has not been assigned.", this);
                return;
            }

            if (sfxSource != null)
                sfxSource.PlayOneShot(clip);
        }

        public void SetMasterVolume(float value)
        {
            SetMixerVolume(masterVolumeParameter, "MasterVolume", value);
        }

        public void SetMusicVolume(float value)
        {
            SetMixerVolume(musicVolumeParameter, "MusicVolume", value);
        }

        public void SetSfxMasterVolume(float value)
        {
            SetMixerVolume(sfxMasterVolumeParameter, "SfxMasterVolume", value);
        }

        public void SetSfxVolume(float value)
        {
            SetMixerVolume(sfxVolumeParameter, "SfxVolume", value);
        }

        public void SetUIVolume(float value)
        {
            SetMixerVolume(uiVolumeParameter, "UIVolume", value);
        }
        
        private void SetMixerVolume(string parameter, string prefsKey, float value)
        {
            value = Mathf.Clamp01(value);

            // Slider 0 คือปิดเสียง; ค่าอื่นแปลงเป็น decibels
            float decibels = value <= 0.0001f
                ? -80f
                : Mathf.Log10(value) * 20f;

            if (audioMixer == null)
            {
                Debug.LogWarning("Audio Mixer is not assigned.", this);
                return;
            }

            if (!audioMixer.SetFloat(parameter, decibels))
            {
                Debug.LogWarning(
                    $"ไม่พบ Exposed Parameter '{parameter}' ตรวจชื่อใน Audio Mixer",
                    this
                );
                return;
            }

            PlayerPrefs.SetFloat(prefsKey, value);
            PlayerPrefs.Save();
        }

        public void ShowCursor()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        public void LockCursor()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}