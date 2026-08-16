using UnityEngine;

public class Subtitle : MonoBehaviour
{
    [SerializeField] private float zoomSpeed = 4f;
    [SerializeField] private float zoomAmount = 0.15f;
    private RectTransform textRectTransform;
    private Vector3 baseScale;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        textRectTransform = GetComponent<RectTransform>();
        baseScale = textRectTransform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        float wave = Mathf.Sin(Time.unscaledTime * zoomSpeed);
        float scaleOffset = wave * zoomAmount;
        textRectTransform.localScale = baseScale + new Vector3(scaleOffset, scaleOffset, scaleOffset);
    }
}
