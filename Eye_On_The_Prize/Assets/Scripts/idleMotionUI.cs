using UnityEngine;
using UnityEngine.UI;

public class SineScale : MonoBehaviour
{
    [SerializeField]
    private bool scale = true;
    [SerializeField]
    private bool upDownFloat = false;

    public float frequency = 2f;      // How fast it oscillates
    public float scaleAmplitude = 0.2f;    // How big the scale change is
    public float yAmplitude = 20.0f;    // How big the y change is
    public float baseScale = 1f;      // Base scale size

    [Range(0.0f, 2.0f)]
    public float scaleSinOffset;
    [Range(0.0f, 2.0f)]
    public float ySinOffset;

    private RectTransform rectTransform;
    private Vector3 initialScale;
    private Vector3 initialPosition;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        initialScale = new Vector3(baseScale, baseScale, baseScale);
        initialPosition = new Vector3(rectTransform.position.x, rectTransform.position.y, rectTransform.position.z);
    }

    void Update()
    {
        if (scale)
        {
            float scaleOffset = Mathf.Sin((Time.time + scaleSinOffset) * frequency) * scaleAmplitude;
            rectTransform.localScale = initialScale + new Vector3(scaleOffset, scaleOffset, 0f);
        }

        if (upDownFloat)
        {
            float yOffset = Mathf.Sin((Time.time + ySinOffset) * frequency) * yAmplitude;
            rectTransform.position = new Vector2(initialPosition.x, initialPosition.y + yOffset);
        }
    }
}