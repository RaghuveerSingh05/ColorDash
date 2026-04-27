using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class NeonProgressBar : MonoBehaviour
{
    [Header("References")]
    public Slider progressSlider;
    public Image fillImage;
    public TextMeshProUGUI percentageText;
    
    [Header("Neon Colors")]
    public Color startColor = new Color(0f, 1f, 1f);      // Cyan
    public Color midColor = new Color(0f, 1f, 0.5f);      // Green-Cyan
    public Color endColor = new Color(0f, 1f, 0f);        // Green
    
    [Header("Animation")]
    public float fillSpeed = 3f;
    public bool animateFill = true;
    
    private float targetProgress = 0f;
    private float currentProgress = 0f;
    
    void Start()
    {
        // Find slider reference if not set
        if (progressSlider == null)
            progressSlider = GetComponent<Slider>();
        
        // Configure slider correctly
        if (progressSlider != null)
        {
            progressSlider.minValue = 0f;
            progressSlider.maxValue = 1f;
            progressSlider.value = 0f;
        }
        
        // Find fill image if not set
        if (fillImage == null && progressSlider != null)
        {
            Transform fillTransform = progressSlider.transform.Find("Fill Area/Fill");
            if (fillTransform != null)
                fillImage = fillTransform.GetComponent<Image>();
        }
        
        // IMPORTANT: Configure fill image for proper filling
        if (fillImage != null)
        {
            fillImage.type = Image.Type.Sliced;
            fillImage.fillMethod = Image.FillMethod.Horizontal;
            fillImage.fillOrigin = 0; // Left
            fillImage.fillAmount = 0f;
        }
        
        // Set initial color
        if (fillImage != null)
            fillImage.color = startColor;
        
        // Start at 0
        SetProgress(0f);
        if (progressSlider != null)
            progressSlider.value = 0f;
    }
    
    void Update()
    {
        if (animateFill)
        {
            // Smoothly animate towards target
            currentProgress = Mathf.Lerp(currentProgress, targetProgress, Time.deltaTime * fillSpeed);
            
            // Update slider value
            if (progressSlider != null)
                progressSlider.value = currentProgress;
            
            // Update fill image directly (for reliability)
            if (fillImage != null)
                fillImage.fillAmount = currentProgress;
            
            // Update color based on progress
            UpdateFillColor(currentProgress);
            
            // Update percentage text
            if (percentageText != null)
                percentageText.text = Mathf.RoundToInt(currentProgress * 100) + "%";
        }
    }
    
    public void SetProgress(float progress)
    {
        targetProgress = Mathf.Clamp01(progress);
        
        if (!animateFill)
        {
            currentProgress = targetProgress;
            if (progressSlider != null)
                progressSlider.value = currentProgress;
            if (fillImage != null)
                fillImage.fillAmount = currentProgress;
            UpdateFillColor(currentProgress);
        }
    }
    
    void UpdateFillColor(float progress)
    {
        if (fillImage == null) return;
        
        Color newColor;
        
        // Gradient from Cyan → Green-Cyan → Green
        if (progress < 0.5f)
        {
            float t = progress / 0.5f;
            newColor = Color.Lerp(startColor, midColor, t);
        }
        else
        {
            float t = (progress - 0.5f) / 0.5f;
            newColor = Color.Lerp(midColor, endColor, t);
        }
        
        fillImage.color = newColor;
    }
    
    public void ResetBar()
    {
        targetProgress = 0f;
        currentProgress = 0f;
        if (progressSlider != null)
            progressSlider.value = 0f;
        if (fillImage != null)
            fillImage.fillAmount = 0f;
        UpdateFillColor(0f);
    }
}