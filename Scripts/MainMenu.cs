using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainMenu : MonoBehaviour
{
    [Header("UI References")]
    public Button playButton;
    public Button aboutButton;
    public Button quitButton;
    public GameObject aboutPanel;
    public Button closeAboutButton;
    
    [Header("Scene Names")]
    public string loadingSceneName = "LoadingScene";
    
    void Start()
    {
        // Setup button listeners with sound
        if (playButton != null)
            playButton.onClick.AddListener(() => {
                AudioManager.Instance?.PlayButtonClick();
                PlayGame();
            });
        
        if (aboutButton != null)
            aboutButton.onClick.AddListener(() => {
                AudioManager.Instance?.PlayButtonClick();
                OpenAbout();
            });
        
        if (quitButton != null)
            quitButton.onClick.AddListener(() => {
                AudioManager.Instance?.PlayButtonClick();
                QuitGame();
            });
        
        if (closeAboutButton != null)
            closeAboutButton.onClick.AddListener(() => {
                AudioManager.Instance?.PlayButtonClick();
                CloseAbout();
            });
        
        // Hide about panel
        if (aboutPanel != null)
            aboutPanel.SetActive(false);
        
        // Unlock cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        
        // Play menu music
        AudioManager.Instance?.PlayMainMenuMusic();
    }
    
    public void PlayGame()
    {
        Debug.Log("Starting game... loading loading scene");
        SceneManager.LoadScene(loadingSceneName);
    }
    
    public void OpenAbout()
    {
        if (aboutPanel != null)
            aboutPanel.SetActive(true);
    }
    
    public void CloseAbout()
    {
        if (aboutPanel != null)
            aboutPanel.SetActive(false);
    }
    
    public void QuitGame()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
}