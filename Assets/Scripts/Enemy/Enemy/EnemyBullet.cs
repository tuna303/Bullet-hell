using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float speed = 10f;
    [Tooltip("Sau bao lâu thì đạn tự biến mất nếu không trúng ai")]
    public float lifeTime = 3f;

    private bool isReleased;

    private void OnEnable()
    {
        isReleased = false;
    }

    public void FireBullet()
    {
        // Bắt đầu đếm ngược thời gian sống của viên đạn ngay khi rời nòng
        Invoke(nameof(ReturnToPool), lifeTime);
    }

    void Update()
    {
        // Đạn luôn bay tới phía trước (trục X)
        transform.Translate(Vector3.right * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Nếu trúng Player hoặc Wall
        if (collision.CompareTag("Player") || collision.CompareTag("Wall"))
        {
            ReturnToPool();
        }
    }

    private void ReturnToPool()
    {
        // Chống release trùng lặp (bullet chạm 2 collider cùng lúc)
        if (isReleased) return;
        isReleased = true;

        // Hủy bộ đếm thời gian (phòng trường hợp đạn chạm tường trước khi hết lifeTime)
        CancelInvoke();

        // Trả đạn về đúng kho của kẻ địch
        if (EnemyBulletPool.Instance != null && EnemyBulletPool.Instance.pool != null)
        {
            EnemyBulletPool.Instance.pool.Release(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
