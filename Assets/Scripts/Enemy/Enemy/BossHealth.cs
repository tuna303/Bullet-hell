using UnityEngine;

/// <summary>
/// Quản lý máu cho Boss.
/// Tích hợp với RoomInfo: khi boss chết sẽ gọi roomOwner.NotifyEnemyDied() để mở cửa.
/// Khi boss chết cũng kích hoạt cổng dịch chuyển (nếu được gán).
/// </summary>
public class BossHealth : MonoBehaviour
{
    [Header("Máu Boss")]
    public int maxHealth = 50;
    private int currentHealth;

    [Header("Cổng dịch chuyển")]
    [Tooltip("GameObject cổng sẽ được SetActive(true) khi boss chết.")]
    public GameObject portalGate;

    [HideInInspector] public RoomInfo roomOwner;

    void Start()
    {
        currentHealth = maxHealth;
    }

    /// <summary>
    /// Gọi từ đạn Player (tag "Bullet") hoặc bất kỳ nguồn sát thương nào.
    /// </summary>
    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;

        if (currentHealth <= 0)
        {
            // Báo cho RoomInfo biết boss đã chết
            if (roomOwner != null)
            {
                roomOwner.NotifyEnemyDied();
            }

            // Kích hoạt cổng dịch chuyển
            if (portalGate != null)
            {
                portalGate.SetActive(true);
            }

            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Trả về % máu hiện tại (0f -> 1f) để HealthBar hiển thị.
    /// </summary>
    public float GetHealthPercent()
    {
        return (float)currentHealth / maxHealth;
    }

    /// <summary>
    /// Khi đạn Player (tag "Bullet") bắn trúng boss.
    /// Lưu ý: Bullet tự xử lý ReturnToPool() khi chạm tag "Enemy",
    /// nên ở đây chỉ cần nhận damage, không tự ý trả pool.
    /// </summary>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            // Mặc định 1 damage (có thể tăng sau nếu Bullet có field damage)
            TakeDamage(1);
        }
    }
}