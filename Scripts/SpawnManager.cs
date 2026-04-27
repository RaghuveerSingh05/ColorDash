using UnityEngine;
using System.Collections;

public class SpawnManager : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject obstaclePrefab;
    
    [Header("Spawn Settings")]
    public float spawnZPosition = 25f; // How far ahead to spawn
    public float spawnIntervalMin = 1f;
    public float spawnIntervalMax = 3f;
    
    [Header("Difficulty")]
    public float currentDifficulty = 1f;
    public float difficultyIncreaseRate = 0.1f; // Per second
    
    private float nextSpawnTime;
    private Transform playerTransform;
    
    void Start()
    {
        // Find the player
        playerTransform = GameObject.FindGameObjectWithTag("Player")?.transform;
        
        // Add Player tag to your Player object
        if (playerTransform == null)
        {
            Debug.LogError("Can't find player! Make sure Player has 'Player' tag");
        }
        
        // Start spawning
        nextSpawnTime = Time.time + 1f;
    }
    
    void Update()
    {
        // Increase difficulty over time
        currentDifficulty += difficultyIncreaseRate * Time.deltaTime;
        
        // Check if it's time to spawn
        if (Time.time >= nextSpawnTime)
        {
            SpawnObstacle();
            
            // Calculate next spawn time (lower interval = harder)
            float currentMin = Mathf.Max(0.5f, spawnIntervalMin / currentDifficulty);
            float currentMax = Mathf.Max(0.7f, spawnIntervalMax / currentDifficulty);
            nextSpawnTime = Time.time + Random.Range(currentMin, currentMax);
        }
    }
    
    void SpawnObstacle()
    {
        if (obstaclePrefab == null || playerTransform == null) return;
        
        // Random X position (lane system)
        float xPos = Random.Range(-3f, 3f);
        
        // Spawn position
        Vector3 spawnPos = new Vector3(xPos, 0f, playerTransform.position.z + spawnZPosition);
        
        // Create obstacle
        GameObject newObstacle = Instantiate(obstaclePrefab, spawnPos, Quaternion.identity);
        
        // Auto-destroy after it passes the player (to save memory)
        Destroy(newObstacle, 8f);
    }
}