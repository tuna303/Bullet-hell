using UnityEngine;
using System; // [THÊM VÀO] Khai báo thư viện System để dùng Action

public class PlayerHealth : MonoBehaviour
{
    [Header("Chỉ số Sinh tồn")]
    public int maxHealth = 3;
    public int currentHealth;
    public GameObject deathPrefab;
    [Header("Khung Bất tử (i-frame)")]
    public float invincibleDuration = 0.25f; // Ngắn hơn roll (0.4s)

    // [THÊM VÀO] Loa phát thanh: Gửi đi 2 con số (Máu hiện tại, Máu tối đa)
    public static event Action<int, int> OnHealthChanged; 
    // Su kien phat ra khi player chet de UI biet ma hien "You Died"
    public static event Action OnPlayerDied;

    private PlayerController playerController;
    private Animator animator;
    private float invincibleTimer;
    private bool isInvincible;

    void Start()
    {
        currentHealth = maxHealth;
        playerController = GetComponent<PlayerController>();
        animator = GetComponent<Animator>();
        
        // [THÊM VÀO] Phát thông báo lần đầu tiên khi vào game để UI vẽ trái tim
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    void Update()
    {
        // Đếm ngược i-frame
        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer <= 0f)
            {
                isInvincible = false;
                // Tắt animation hurt + force về idle ngay lập tức
                if (animator != null)
                {
                    animator.SetBool("isHurt", false);
                    animator.Play("Player_Idle", 0, 0f); // Dừng khẩn cấp, về idle ngay
                }
            }
        }
    }

    public void TakeDamage(int damageAmount)
    {
        // Đang roll hoặc đang trong i-frame thì miễn nhiễm sát thương
        if (playerController != null && playerController.isRolling) return;
        if (isInvincible) return;

        currentHealth -= damageAmount;
        
        // [THÊM VÀO] Phát thông báo máu đã giảm
        OnHealthChanged?.Invoke(currentHealth, maxHealth);

        if (currentHealth <= 0)
        {
            Die();
            return;
        }

        // Kích hoạt i-frame + animation bị thương
        StartInvincibleFrame();
    }

    /// <summary>
    /// Bật i-frame ngắn, chạy animation isHurt
    /// </summary>
    private void StartInvincibleFrame()
    {
        isInvincible = true;
        invincibleTimer = invincibleDuration;
        // Dùng Bool thay vì Trigger để giữ animation suốt i-frame, không bị state khác ghi đè
        animator?.SetBool("isHurt", true);
    }

    public void Heal(int healAmount)
    {
        currentHealth += healAmount;
        if (currentHealth > maxHealth) currentHealth = maxHealth;
        
        // [THÊM VÀO] Phát thông báo máu đã tăng
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    public void IncreaseMaxHealth(int extraHealth)
    {
        maxHealth += extraHealth;
        currentHealth += extraHealth; 
        
        // [THÊM VÀO] Phát thông báo giới hạn máu đã tăng
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    private void Die()
    {
        gameObject.SetActive(false);
        if (deathPrefab != null) deathPrefab.SetActive(false);
        // Phat su kien chet de UI hien "You Died"
        OnPlayerDied?.Invoke();
    }

    /// <summary>
    /// Reset mau cua player khi retry level. Goi tu DungeonGenerator.ResetLevel()
    /// </summary>
    public void ResetHealth()
    {
        currentHealth = maxHealth;
        isInvincible = false;
        invincibleTimer = 0f;
        // Tat animation hurt neu dang bat
        if (animator != null)
        {
            animator.SetBool("isHurt", false);
            animator.Play("Player_Idle", 0, 0f);
        }
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }
    
    // (Giữ nguyên các hàm OnTriggerEnter2D và OnCollisionEnter2D)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") || collision.CompareTag("EnemyBullet"))
       
        {
            DamageDealer damageDealer = collision.GetComponent<DamageDealer>();
            int damage = damageDealer != null ? damageDealer.damageAmount : 1;
            TakeDamage(damage);
        }
    }
}