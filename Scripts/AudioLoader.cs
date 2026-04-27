using UnityEngine;

public class AudioLoader : MonoBehaviour
{
    public GameObject audioManagerPrefab;
    
    void Awake()
    {
        // Check if AudioManager already exists
        if (AudioManager.Instance == null)
        {
            // Create AudioManager from prefab
            if (audioManagerPrefab != null)
            {
                Instantiate(audioManagerPrefab);
                Debug.Log("AudioManager created from prefab");
            }
            else
            {
                Debug.LogError("AudioManager prefab not assigned to AudioLoader!");
            }
        }
        else
        {
            Debug.Log("AudioManager already exists");
        }
    }
}