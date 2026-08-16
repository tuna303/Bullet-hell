using UnityEngine;

/// <summary>
/// Đạn của Boss state Đỏ.
/// Bay thẳng theo hướng cố định, gây damage khi chạm Player.
/// Yêu cầu: Tag "EnemyBullet", Collider2D (IsTrigger), DamageDealer component.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class BossBullet : MonoBehaviour
{
    [HideInInspector] public Vector2 moveDirection;
    [HideInInspector] public float moveSpeed;

    public float lifetime = 5f;
    private float lifeTimer;

    private Rigidbody2D rb;
    private DamageDealer damageDealer;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        damageDealer = GetComponent<DamageDealer>();
    }

    void Start()
    {
        lifeTimer = lifetime;
    }

    /// <summary>Gọi từ BossController.FireCircularBullets()</summary>
    public void Initialize(Vector2 dir, float speed, int damage)
    {
        moveDirection = dir;
        moveSpeed = speed;
        rb.linearVelocity = dir * speed;

        // Gán damage vào DamageDealer để PlayerHealth nhận
        if (damageDealer != null)
        {
            damageDealer.damageAmount = damage;
        }
    }

    void Update()
    {
        lifeTimer -= Time.deltaTime;
        if (lifeTimer <= 0)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
}