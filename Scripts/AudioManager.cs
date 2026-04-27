using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    
    [Header("Audio Sources")]
    public AudioSource musicSource;
    public AudioSource sfxSource;
    
    [Header("Sound Effects")]
    public AudioClip colorSwitchSound;
    public AudioClip matchSuccessSound;
    public AudioClip gameOverSound;
    public AudioClip buttonClickSound;
    public AudioClip obstacleDestroySound;
    
    [Header("Background Music")]
    public AudioClip mainMenuMusic;
    public AudioClip gameMusic;
    
    private string currentScene = "";
    
    void Awake()
    {
        // Singleton pattern - only one AudioManager exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("AudioManager created and will persist across scenes");
        }
        else
        {
            Debug.Log("Duplicate AudioManager destroyed");
            Destroy(gameObject);
            return;
        }
    }
    
    void Start()
    {
        // Set volume levels
        if (musicSource != null)
        {
            musicSource.volume = 0.4f;
            musicSource.loop = true;
        }
        if (sfxSource != null)
        {
            sfxSource.volume = 0.7f;
            sfxSource.loop = false;
        }
        
        Debug.Log("AudioManager initialized");
    }
    
    void OnEnable()
    {
        // Subscribe to scene load event
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
    }
    
    void OnDisable()
    {
        // Unsubscribe to prevent memory leaks
        UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    
    void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode)
    {
        Debug.Log("Scene loaded: " + scene.name);
        
        // Fix for restart - ensure audio sources still work
        if (sfxSource != null)
        {
            sfxSource.Stop(); // Reset any stuck sounds
        }
        
        // Play appropriate music based on scene
        if (scene.name == "MainMenu")
        {
            PlayMainMenuMusic();
        }
        else if (scene.name == "GameScene")
        {
            PlayGameMusic();
        }
    }
    
    public void PlayColorSwitch()
    {
        if (colorSwitchSound != null && sfxSource != null)
        {
            sfxSource.PlayOneShot(colorSwitchSound, 0.6f);
            Debug.Log("Playing color switch sound");
        }
        else
        {
            Debug.LogWarning("Color switch sound or SFX source missing!");
        }
    }
    
    public void PlayMatchSuccess()
    {
        if (matchSuccessSound != null && sfxSource != null)
        {
            sfxSource.PlayOneShot(matchSuccessSound, 0.8f);
            Debug.Log("Playing match success sound");
        }
        else
        {
            Debug.LogWarning("Match success sound or SFX source missing!");
        }
    }
    
    public void PlayGameOver()
    {
        if (gameOverSound != null && sfxSource != null)
        {
            sfxSource.PlayOneShot(gameOverSound, 1f);
            Debug.Log("Playing game over sound");
        }
        else
        {
            Debug.LogWarning("Game over sound or SFX source missing!");
        }
    }
    
    public void PlayButtonClick()
    {
        if (buttonClickSound != null && sfxSource != null)
        {
            sfxSource.PlayOneShot(buttonClickSound, 0.5f);
        }
    }
    
    public void PlayObstacleDestroy()
    {
        if (obstacleDestroySound != null && sfxSource != null)
        {
            sfxSource.PlayOneShot(obstacleDestroySound, 0.5f);
        }
    }
    
    public void PlayMainMenuMusic()
    {
        if (musicSource != null && mainMenuMusic != null)
        {
            // Only restart music if it's different
            if (musicSource.clip != mainMenuMusic)
            {
                musicSource.clip = mainMenuMusic;
                musicSource.Play();
                Debug.Log("Playing main menu music");
            }
            else if (!musicSource.isPlaying)
            {
                musicSource.Play();
            }
        }
        else
        {
            Debug.LogWarning("Main menu music or music source missing!");
        }
    }
    
    public void PlayGameMusic()
    {
        if (musicSource != null && gameMusic != null)
        {
            // Only restart music if it's different
            if (musicSource.clip != gameMusic)
            {
                musicSource.clip = gameMusic;
                musicSource.Play();
                Debug.Log("Playing game music");
            }
            else if (!musicSource.isPlaying)
            {
                musicSource.Play();
            }
        }
        else
        {
            Debug.LogWarning("Game music or music source missing!");
        }
    }
    
    public void StopMusic()
    {
        if (musicSource != null && musicSource.isPlaying)
            musicSource.Stop();
    }
}