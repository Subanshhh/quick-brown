using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Sources")]
    [SerializeField] private AudioSource MusicSource;
    [SerializeField] private AudioSource SFXSource;

    [Header("Music Clips")]
    public AudioClip mainmenuMusic;
    public AudioClip gameplayMusic;

    [Header("Gameplay SFX")]
    public AudioClip deathSFX;
    public AudioClip portalEnterSFX;
    public AudioClip levelCompleteSFX;
    public AudioClip[] foxMovementSFX; // multiple variations (1–4)

    [Header("UI SFX")]
    public AudioClip uiClickSFX;

    private static AudioManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MainMenu" || scene.name == "Settings" || scene.name == "Credits" || scene.name == "LevelSelector")
            SwitchMusic(mainmenuMusic);
        else
            SwitchMusic(gameplayMusic);
    }

    private void SwitchMusic(AudioClip newClip)
    {
        if (MusicSource.clip == newClip) return;
        MusicSource.clip = newClip;
        MusicSource.Play();
    }

    // 🔊 Public functions you can call from anywhere:
    public static void PlayDeathSFX() => instance?.PlaySFX(instance.deathSFX);
    public static void PlayUIClick() => instance?.PlaySFX(instance.uiClickSFX);
    public static void PlayPortalEnter() => instance?.PlaySFX(instance.portalEnterSFX);
    public static void PlayLevelComplete() => instance?.PlaySFX(instance.levelCompleteSFX);

    private static float lastFoxMoveTime = 0f;
    private static float foxMoveCooldown = 0.5f; // seconds between movement sounds

    public static void PlayFoxMove()
    {
        if (instance == null || instance.foxMovementSFX == null || instance.foxMovementSFX.Length == 0)
            return;

        if (Time.time - lastFoxMoveTime < foxMoveCooldown)
            return; // still on cooldown

        lastFoxMoveTime = Time.time; // reset timer

        int index = Random.Range(0, instance.foxMovementSFX.Length);
        instance.PlaySFX(instance.foxMovementSFX[index]);
    }


    // Core SFX playback
    private void PlaySFX(AudioClip clip)
    {
        if (clip != null)
            SFXSource.PlayOneShot(clip);
    }
}
