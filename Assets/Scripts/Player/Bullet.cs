using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour
{
    [Header("Chỉ số Đạn")]
    public float speed = 15f;
    public float lifeTime = 2f; // Thời gian sống tối đa (tránh đạn bay vô tận)

    private Rigidbody2D rb;
    private float currentLifeTime;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        
        // Đạn trong Gungeon bay thẳng không rớt
        rb.gravityScale = 0f; 
    }

    // Hàm OnEnable tự động chạy mỗi khi đạn được rút ra khỏi Pool (SetActive = true)
    void OnEnable()
    {
       
    }

    void Update()
    {
        currentLifeTime -= Time.deltaTime;
        if (currentLifeTime <= 0)
        {
            // Trả đạn về kho thay vì Destroy
            ReturnToPool();
        }
    }

    // Xử lý va chạm với tường (hoặc quái)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Tạm thời trả đạn về kho nếu chạm bất cứ thứ gì có Trigger
        if (collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Enemy"))
        {
            ReturnToPool();
        }
    }

    private void ReturnToPool()
    {
        // Chặn lỗi nếu Pool chưa kịp load hoặc đã bị xóa
        if (BulletPool.Instance != null && gameObject.activeInHierarchy)
        {
            BulletPool.Instance.pool.Release(gameObject);
        }
    }
    public void FireBullet()
    {
        // Bơm lực đẩy đạn về phía trước mặt (sau khi đã được xoay chuẩn)
        rb.linearVelocity = transform.right * speed;
        
        // Reset lại đồng hồ sự sống
        currentLifeTime = lifeTime;
    }
}