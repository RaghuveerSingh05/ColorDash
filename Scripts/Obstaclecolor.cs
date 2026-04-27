using UnityEngine;

public class ObstacleColor : MonoBehaviour
{
    [Header("Colors")]
    public Color redColor = new Color(1f, 0f, 0f, 1f);
    public Color blueColor = new Color(0f, 0f, 1f, 1f);
    public Color greenColor = new Color(0f, 1f, 0f, 1f);
    
    [Header("Bloom Intensity (Same as Player)")]
    public float redIntensity = 3f;    // Red needs extra boost
    public float blueIntensity = 2.5f;  // Blue is naturally dimmer
    public float greenIntensity = 2.8f; // Green is in the middle
    
    [Header("Random Color")]
    public int currentColorIndex;
    
    private Renderer cubeRenderer;
    private Material material;
    
    void Start()
    {
        cubeRenderer = GetComponent<Renderer>();
        
        // Create UNIQUE material instance for each obstacle
        material = new Material(cubeRenderer.material);
        cubeRenderer.material = material;
        
        // Enable emission for glow
        material.EnableKeyword("_EMISSION");
        
        // Pick random color
        SetRandomColor();
    }
    
    void SetRandomColor()
    {
        // Random color: 0=Red, 1=Blue, 2=Green
        currentColorIndex = Random.Range(0, 3);
        
        Color newColor;
        float intensity;
        
        switch(currentColorIndex)
        {
            case 0:
                newColor = redColor;
                intensity = redIntensity;
                break;
            case 1:
                newColor = blueColor;
                intensity = blueIntensity;
                break;
            case 2:
                newColor = greenColor;
                intensity = greenIntensity;
                break;
            default:
                newColor = Color.white;
                intensity = 2f;
                break;
        }
        
        // Apply color and glow
        material.color = newColor;
        material.SetColor("_EmissionColor", newColor * intensity);
        
        // Optional: Debug to verify colors are set
        // Debug.Log($"Obstacle: {newColor} with intensity {intensity}");
    }
    
    public int GetColorIndex()
    {
        return currentColorIndex;
    }
}