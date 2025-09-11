using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource MusicSource;
    [SerializeField] private AudioSource SFXSource;
    [SerializeField] private AudioSource MasterSource;

    public AudioClip mainmenuMusic;
    public AudioClip gameplayMusic;

    private static AudioManager instance;

    private void Awake()
    {
        // Make sure only one AudioManager exists
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // keep between scenes
            SceneManager.sceneLoaded += OnSceneLoaded; // listen for scene changes
        }
        else
        {
            Destroy(gameObject); // prevent duplicates
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MainMenu" || scene.name == "Settings" || scene.name == "Credits" || scene.name == "LevelSelector")
        {
            SwitchMusic(mainmenuMusic);
        }
        else
        {
            SwitchMusic(gameplayMusic);
        }
    }

    private void SwitchMusic(AudioClip newClip)
    {
        if (MusicSource.clip == newClip) return; // don’t restart if same track
        MusicSource.clip = newClip;
        MusicSource.Play();
    }
}
