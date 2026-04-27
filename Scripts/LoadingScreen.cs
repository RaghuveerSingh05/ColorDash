using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI loadingText;
    public Slider progressBar;
    
    [Header("Settings")]
    public string gameSceneName = "GameScene";
    public float minLoadTime = 1.5f;
    
    private AsyncOperation asyncLoad;
    private float startTime;
    
    void Start()
    {
        startTime = Time.time;
        StartCoroutine(LoadGameScene());
    }
    
    IEnumerator LoadGameScene()
    {
        asyncLoad = SceneManager.LoadSceneAsync(gameSceneName);
        asyncLoad.allowSceneActivation = false;
        
        while (!asyncLoad.isDone)
        {
            float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);
            
            if (progressBar != null)
                progressBar.value = progress;
            
            if (loadingText != null)
                loadingText.text = "LOADING " + Mathf.RoundToInt(progress * 100) + "%";
            
            if (asyncLoad.progress >= 0.9f && Time.time - startTime >= minLoadTime)
            {
                yield return new WaitForSeconds(0.2f);
                asyncLoad.allowSceneActivation = true;
            }
            
            yield return null;
        }
    }
}