using UnityEngine;
using System.Collections;

public class PlayerColorScript : MonoBehaviour
{
    [Header("Colors")]
    public Color red = new Color(1f, 0f, 0f, 1f);
    public Color blue = new Color(0f, 0f, 1f, 1f);
    public Color green = new Color(0f, 1f, 0f, 1f);
    
    [Header("Bloom Intensity")]
    public float redIntensity = 3f;
    public float blueIntensity = 2.5f;
    public float greenIntensity = 2.8f;

    [Header("Current State")]
    public int currentcolorindex = 0;
    
    [Header("Color-Matched Particles")]
    public GameObject redParticlePrefab;
    public GameObject blueParticlePrefab;
    public GameObject greenParticlePrefab;

    private Renderer cuberenderer;
    private Material material;

    void Start()
    {
        cuberenderer = GetComponent<Renderer>();
        material = new Material(cuberenderer.material);
        cuberenderer.material = material;
        
        material.EnableKeyword("_EMISSION");
        SetColor(0);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            SwitchToNextColor();
        }
    }

    void SwitchToNextColor()
    {
        currentcolorindex = (currentcolorindex + 1) % 3;
        SetColor(currentcolorindex);
        
        if (AudioManager.Instance != null)
        AudioManager.Instance.PlayColorSwitch();

        
        
    }

    public void SetColor(int index)
    {
        Color newColor;
        float intensity;
        
        switch(index)
        {
            case 0: 
                newColor = red;
                intensity = redIntensity;
                break;
            case 1: 
                newColor = blue;
                intensity = blueIntensity;
                break;
            case 2: 
                newColor = green;
                intensity = greenIntensity;
                break;
            default: 
                newColor = Color.white;
                intensity = 2f;
                break;
        }
        
        material.color = newColor;
        material.SetColor("_EmissionColor", newColor * intensity);
    }

    IEnumerator FlashColor()
    {
        material.SetColor("_EmissionColor", Color.white * 3);
        yield return new WaitForSeconds(0.1f);
        
        Color currentColor = Color.white;
        float intensity = 2f;
        switch(currentcolorindex)
        {
            case 0: currentColor = red; intensity = redIntensity; break;
            case 1: currentColor = blue; intensity = blueIntensity; break;
            case 2: currentColor = green; intensity = greenIntensity; break;
        }
        material.SetColor("_EmissionColor", currentColor * intensity);
    }

    public int GetCurrentColorIndex()
    {
        return currentcolorindex;
    }
    
    void SpawnColorMatchedParticles(int colorIndex)
    {
        GameObject particleToSpawn = null;
        
        switch(colorIndex)
        {
            case 0:
                particleToSpawn = redParticlePrefab;
                break;
            case 1:
                particleToSpawn = blueParticlePrefab;
                break;
            case 2:
                particleToSpawn = greenParticlePrefab;
                break;
        }
        
        if (particleToSpawn != null)
        {
            GameObject effect = Instantiate(particleToSpawn, transform.position, Quaternion.identity);
            ParticleSystem ps = effect.GetComponent<ParticleSystem>();
            if (ps != null) ps.Play();
            Destroy(effect, 1f);
        }
    }
    
    void OnTriggerEnter(Collider other)
{
    if (other.CompareTag("Obstacle"))
    {
        ObstacleColor obstacle = other.GetComponent<ObstacleColor>();
        
        if (obstacle != null)
        {
            if (obstacle.GetColorIndex() == currentcolorindex)
            {
                SpawnColorMatchedParticles(currentcolorindex);
                
                // Play sounds with null check
                if (AudioManager.Instance != null)
                {
                    AudioManager.Instance.PlayMatchSuccess();
                    AudioManager.Instance.PlayObstacleDestroy();
                }
                
                Destroy(other.gameObject);
                if (GameManager.Instance != null)
                    GameManager.Instance.AddScore(10);
            }
            else
            {
                if (GameManager.Instance != null)
                    GameManager.Instance.GameOver();
            }
        }
    }
}
}