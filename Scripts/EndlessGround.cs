using UnityEngine;
using System.Collections.Generic;

public class EndlessGround : MonoBehaviour
{
    [Header("Ground Settings")]
    public GameObject groundTilePrefab;
    public int tileCount = 3;        // How many tiles active at once
    public float tileLength = 20f;   // Length of each tile (Z-axis)
    
    [Header("References")]
    public Transform player;
    
    private List<GameObject> activeTiles = new List<GameObject>();
    private float spawnZPosition = 0f;
    
    void Start()
    {
        // Find player if not assigned
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player")?.transform;
        
        // Create initial ground tiles
        for (int i = 0; i < tileCount; i++)
        {
            SpawnTile();
        }
    }
    
    void Update()
    {
        // Check if we need to spawn more tiles
        if (player != null && activeTiles.Count > 0)
        {
            float playerZ = player.position.z;
            float lastTileZ = activeTiles[activeTiles.Count - 1].transform.position.z;
            
            // If player is near the end of the last tile, spawn new one
            if (lastTileZ - playerZ < tileLength * 0.7f)
            {
                SpawnTile();
            }
            
            // Remove tiles that are far behind
            if (activeTiles.Count > tileCount + 1)
            {
                GameObject oldestTile = activeTiles[0];
                activeTiles.RemoveAt(0);
                Destroy(oldestTile);
            }
        }
    }
    
    void SpawnTile()
    {
        Vector3 spawnPos = new Vector3(0, -0.5f, spawnZPosition);
        GameObject newTile = Instantiate(groundTilePrefab, spawnPos, Quaternion.identity);
        activeTiles.Add(newTile);
        spawnZPosition += tileLength;
    }
}