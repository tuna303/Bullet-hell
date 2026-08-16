using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Thanh máu Boss kiểu Dark Souls — nằm dưới cùng, giữa màn hình.
/// Tự động ẩn khi boss chưa spawn hoặc đã chết.
/// </summary>
public class BossHealthBar : MonoBehaviour
{
    [Header("UI Slider")]
    [Tooltip("Kéo Slider UI vào đây")]
    public Slider healthSlider;

    [Header("Text tên Boss (tùy chọn)")]
    public Text bossNameText;

    [Header("Target")]
    [Tooltip("BossHealth của boss. Để trống sẽ tự tìm.")]
    public BossHealth targetBossHealth;

    [Header("Hiệu ứng hiện/ẩn")]
    public float fadeSpeed = 3f;
    private CanvasGroup canvasGroup;
    private bool shouldShow;

    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();

        // Tự tìm boss
        if (targetBossHealth == null)
            targetBossHealth = FindAnyObjectByType<BossHealth>();

        if (healthSlider != null)
        {
            healthSlider.minValue = 0f;
            healthSlider.maxValue = 1f;
            healthSlider.value = 0f;
        }

        // Ẩn lúc đầu
        canvasGroup.alpha = 0f;
    }

    void Update()
    {
        // Kiểm tra boss còn sống không
        shouldShow = (targetBossHealth != null && targetBossHealth.gameObject.activeInHierarchy);

        // Fade in/out
        float targetAlpha = shouldShow ? 1f : 0f;
        canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, targetAlpha, fadeSpeed * Time.deltaTime);

        // Cập nhật slider
        if (shouldShow && healthSlider != null)
        {
            healthSlider.value = targetBossHealth.GetHealthPercent();
        }
    }

    /// <summary>
    /// Gán boss từ bên ngoài (vd: khi boss được spawn).
    /// </summary>
    public void SetBoss(BossHealth boss)
    {
        targetBossHealth = boss;
    }
}