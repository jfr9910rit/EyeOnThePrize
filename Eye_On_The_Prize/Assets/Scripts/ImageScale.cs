using UnityEngine;
using UnityEngine.UI;

public class ImageScale : MonoBehaviour
{
    public Image targetImage; // Assign the Image component in the Inspector
    public float pulseSpeed = 2.0f; // Speed of the pulse
    public float pulseAmount = 0.1f; // Amount to pulse

    private Vector3 originalScale;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (targetImage != null)
        {
            originalScale = targetImage.transform.localScale;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (targetImage != null)
        {
            float scale = 1.0f + Mathf.Sin(Time.time * pulseSpeed) * pulseAmount;
            targetImage.transform.localScale = originalScale * scale;
        }
    }
}
