using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [Header("Pause UI")]
    public GameObject pausePanel;
    public GameObject gameUI; // Your score text, etc.
    
    [Header("Key Settings")]
    public KeyCode pauseKey = KeyCode.Escape;
    
    private bool isPaused = false;
    
    void Start()
    {
        // Ensure pause panel is hidden at start
        if (pausePanel != null)
            pausePanel.SetActive(false);
        
        // Unlock cursor for testing (game will lock it when playing)
        // We'll handle cursor in Update
    }
    
    void Update()
    {
        // Check for pause key press
        if (Input.GetKeyDown(pauseKey))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }
    
    public void PauseGame()
    {
        isPaused = true;
        
        // Stop time
        Time.timeScale = 0f;
        
        // Show pause panel
        if (pausePanel != null)
            pausePanel.SetActive(true);
        
        // Hide game UI (optional)
        if (gameUI != null)
            gameUI.SetActive(false);
        
        // Unlock cursor for menu
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        
        // Disable player input (optional)
        DisablePlayerControls(true);
    }
    
    public void ResumeGame()
    {
        isPaused = false;
        
        // Resume time
        Time.timeScale = 1f;
        
        // Hide pause panel
        if (pausePanel != null)
            pausePanel.SetActive(false);
        
        // Show game UI
        if (gameUI != null)
            gameUI.SetActive(true);
        
        // Lock cursor back to game
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        // Re-enable player input
        DisablePlayerControls(false);
    }
    
    public void RestartGame()
    {
        // Resume time first
        Time.timeScale = 1f;
        
        // Reload current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    public void GoToMainMenu()
    {
        // Resume time
        Time.timeScale = 1f;
        
        // Load main menu scene
        SceneManager.LoadScene("MainMenu");
    }
    
    public void QuitGame()
    {
        Time.timeScale = 1f; // Resume before quitting
        
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
    
    void DisablePlayerControls(bool disable)
    {
        // Disable player movement and color switching
#pragma warning disable CS0618 // Type or member is obsolete
        PlayerMovement playerMove = FindObjectOfType<PlayerMovement>();
#pragma warning restore CS0618 // Type or member is obsolete
        if (playerMove != null)
            playerMove.enabled = !disable;
        
#pragma warning disable CS0618 // Type or member is obsolete
        PlayerColorScript playerColor = FindObjectOfType<PlayerColorScript>();
#pragma warning restore CS0618 // Type or member is obsolete
        if (playerColor != null)
            playerColor.enabled = !disable;
    }
    
    void OnDestroy()
    {
        // Ensure time scale resets when scene unloads
        Time.timeScale = 1f;
    }
}