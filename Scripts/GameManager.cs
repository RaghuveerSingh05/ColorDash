using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    [Header("UI References")]
    public TextMeshProUGUI scoreText;
    public GameObject gameOverPanel;
    public TextMeshProUGUI finalScoreText;
    
    [Header("Game State")]
    public int currentScore = 0;
    public bool isGameOver = false;
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }
    
    void Start()
    {
        FindAllUIComponents();
        
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        UpdateScoreUI();
        
        // Start game music with slight delay to ensure AudioManager is ready
        Invoke("StartGameMusic", 0.2f);
    }
    
    void StartGameMusic()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayGameMusic();
            Debug.Log("Game music started");
        }
        else
        {
            Debug.LogWarning("AudioManager.Instance is null, music not started");
        }
    }
    
    void FindAllUIComponents()
    {
        if (scoreText == null)
        {
            GameObject scoreObj = GameObject.Find("ScoreText");
            if (scoreObj != null)
                scoreText = scoreObj.GetComponent<TextMeshProUGUI>();
        }
        
        if (gameOverPanel == null)
        {
            gameOverPanel = GameObject.Find("GameOverPanel");
        }
        
        if (finalScoreText == null && gameOverPanel != null)
        {
            Transform finalText = gameOverPanel.transform.Find("FinalScoreText");
            if (finalText != null)
                finalScoreText = finalText.GetComponent<TextMeshProUGUI>();
        }
        
        Debug.Log($"UI Found - ScoreText: {(scoreText != null)}, Panel: {(gameOverPanel != null)}, FinalText: {(finalScoreText != null)}");
    }
    
    public void AddScore(int points)
    {
        if (isGameOver) return;
        
        currentScore += points;
        UpdateScoreUI();
    }
    
    void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = "SCORE: " + currentScore;
    }
    
    public void GameOver()
    {
        if (isGameOver) return;
        
        isGameOver = true;
        Debug.Log("GAME OVER - Showing panel");
        
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayGameOver();
            Debug.Log("Game over sound played");
        }
        
        FindAllUIComponents();
        
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
            if (finalScoreText != null)
                finalScoreText.text = "FINAL SCORE: " + currentScore;
        }
        
#pragma warning disable CS0618 // Type or member is obsolete
        PlayerMovement playerMove = FindObjectOfType<PlayerMovement>();
#pragma warning restore CS0618 // Type or member is obsolete
        if (playerMove != null)
            playerMove.enabled = false;
        
#pragma warning disable CS0618 // Type or member is obsolete
        SpawnManager spawner = FindObjectOfType<SpawnManager>();
#pragma warning restore CS0618 // Type or member is obsolete
        if (spawner != null)
            spawner.enabled = false;
        
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        
        int highScore = PlayerPrefs.GetInt("HighScore", 0);
        if (currentScore > highScore)
        {
            PlayerPrefs.SetInt("HighScore", currentScore);
            PlayerPrefs.Save();
            Debug.Log("New High Score: " + currentScore);
        }
    }
    
    public void RestartGame()
    {
        Debug.Log("RESTARTING GAME");
        Time.timeScale = 1f;
        
        // Reset game state before reloading
        isGameOver = false;
        currentScore = 0;
        
        // Reload the scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        if (AudioManager.Instance != null)
            AudioManager.Instance.StopMusic();
        SceneManager.LoadScene("MainMenu");
    }
}